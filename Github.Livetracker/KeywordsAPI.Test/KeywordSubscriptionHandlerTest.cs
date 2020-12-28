using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using NUnit.Framework;
using SettingsProxyAPI.Keywords;
using SettingsProxyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace KeywordsAPI.Test
{
    [TestFixture]
    class KeywordSubscriptionHandlerTest
    {
        private Mock<IUserKeywordsRepository> _mockUserKeywordsRepository;
        private Mock<IKeywordUpdatesProvider> _mockKeywordUpdatesProvider;

        [SetUp]
        public void SetUp()
        {
            _mockUserKeywordsRepository = new Mock<IUserKeywordsRepository>();
            _mockKeywordUpdatesProvider = new Mock<IKeywordUpdatesProvider>();
        }

        [Test]
        public void OnConnected()
        {
            // Arrange
            User user = new User { Id = 1 };
            Keyword keyword = new Keyword { Word = "word", Source = "source" };

            _mockUserKeywordsRepository
                .Setup(ukr => ukr.GetAllUserKeywords(user.Id))
                .Returns(new List<Keyword> { keyword });
            _mockKeywordUpdatesProvider
                .Setup(kup => kup.GetKeywordSequence(It.Is<KeywordRequest>(kr => kr.Keyword.Equals(keyword.Word) && kr.Source.Equals(keyword.Source))))
                .Returns(new List<KeywordOutput> { new KeywordOutput { Keyword = keyword.Word, Source = keyword.Source } }.ToObservable());

            KeywordSubscriptionHandler keywordSubscriptionHandler =
                new KeywordSubscriptionHandler(_mockUserKeywordsRepository.Object, _mockKeywordUpdatesProvider.Object);

            IObservable<KeywordSubscriptionRequest> userKeywordRequests =
                new List<KeywordSubscriptionRequest> { new KeywordSubscriptionRequest { OperationType = OperationType.Connected } }
                .ToObservable();

            // Act, Assert
            keywordSubscriptionHandler.Handle(user, userKeywordRequests)
                .Do(onNext: message =>
                {
                    JSchemaGenerator schemaGenerator = new JSchemaGenerator();
                    JSchema schema = schemaGenerator.Generate(typeof(KeywordOutput));
                    JObject keywordOutputObj = JObject.Parse(message);
                    Assert.That(keywordOutputObj.IsValid(schema));

                    KeywordOutput keywordOutput = JsonConvert.DeserializeObject<KeywordOutput>(message);
                    Assert.That(keywordOutput.Keyword.Equals(keyword.Word));
                    Assert.That(keywordOutput.Source.Equals(keyword.Source));
                })
                .Subscribe();
        }

        [Test]
        public void OnSubscribe()
        {
            // Arrange
            User user = new User { Id = 1 };
            KeywordSubscriptionRequest keywordRequest = 
                new KeywordSubscriptionRequest 
                { 
                    OperationType = OperationType.Subscribe,
                    Keyword = "word", 
                    Source = "source" 
                };
            List<Keyword> userKeywords = new List<Keyword>();

            _mockUserKeywordsRepository
                .Setup(ukr => ukr.AddKeywordForUser(user.Id, keywordRequest.Keyword, keywordRequest.Source))
                .Callback(() => userKeywords.Add(new Keyword { Word = keywordRequest.Keyword, Source = keywordRequest.Source }));
            _mockUserKeywordsRepository
                .Setup(ukr => ukr.GetAllUserKeywords(user.Id))
                .Returns(userKeywords);
            _mockKeywordUpdatesProvider
                .Setup(kup => kup.GetKeywordSequence(keywordRequest))
                .Returns(new List<KeywordOutput> { new KeywordOutput { Keyword = keywordRequest.Keyword, Source = keywordRequest.Source } }.ToObservable());

            KeywordSubscriptionHandler keywordSubscriptionHandler =
                new KeywordSubscriptionHandler(_mockUserKeywordsRepository.Object, _mockKeywordUpdatesProvider.Object);

            IObservable<KeywordSubscriptionRequest> userKeywordRequests =
                new List<KeywordSubscriptionRequest> { keywordRequest }.ToObservable();

            // Act, Assert
            keywordSubscriptionHandler.Handle(user, userKeywordRequests)
                .Do(onNext: message =>
                {
                    JSchemaGenerator schemaGenerator = new JSchemaGenerator();
                    JSchema schema = schemaGenerator.Generate(typeof(KeywordOutput));
                    JObject keywordOutputObj = JObject.Parse(message);
                    Assert.That(keywordOutputObj.IsValid(schema));

                    KeywordOutput keywordOutput = JsonConvert.DeserializeObject<KeywordOutput>(message);
                    Assert.That(keywordOutput.Keyword.Equals(keywordRequest.Keyword));
                    Assert.That(keywordOutput.Source.Equals(keywordRequest.Source));

                    Assert.That(userKeywords.Count, Is.EqualTo(1));
                })
                .Subscribe();
        }

        [Test]
        public void OnUnsubscribe_WithFromKeywordsRemoved()
        {
            // Arrange
            User user = new User { Id = 1 };
            KeywordSubscriptionRequest keywordRequest =
                new KeywordSubscriptionRequest
                {
                    OperationType = OperationType.Unsubscribe,
                    Keyword = "word",
                    Source = "source"
                };
            Keyword anotherKeyword =
                new Keyword
                {
                    Word = "another",
                    Source = "local"
                };
            List<Keyword> userKeywords = 
                new List<Keyword> 
                { 
                    new Keyword 
                    { 
                        Word = keywordRequest.Keyword, 
                        Source = keywordRequest.Source 
                    },
                    anotherKeyword
                };
            bool wasKeywordSequenceRemoved = false;

            _mockUserKeywordsRepository
                .Setup(ukr => ukr.RemoveKeywordForUser(user.Id, keywordRequest.Keyword, keywordRequest.Source))
                .Callback(() => userKeywords.RemoveAll(k => k.Word.Equals(keywordRequest.Keyword) && k.Source.Equals(keywordRequest.Source)))
                .Returns((true, false));
            _mockKeywordUpdatesProvider
                .Setup(kup => kup.RemoveKeywordSequence(keywordRequest))
                .Callback(() => wasKeywordSequenceRemoved = true);

            KeywordSubscriptionHandler keywordSubscriptionHandler =
                new KeywordSubscriptionHandler(_mockUserKeywordsRepository.Object, _mockKeywordUpdatesProvider.Object);

            IObservable<KeywordSubscriptionRequest> userKeywordRequests =
                new List<KeywordSubscriptionRequest> { keywordRequest }.ToObservable();

            // Act
            keywordSubscriptionHandler.Handle(user, userKeywordRequests).Subscribe();

            // Assert
            Assert.That(userKeywords.Count, Is.EqualTo(1));
            Assert.That(userKeywords.Any(k => k.Word.Equals(keywordRequest.Keyword) && k.Source.Equals(keywordRequest.Source)), Is.False);
            Assert.That(wasKeywordSequenceRemoved, Is.False);
        }

        [Test]
        public void OnUnsubscribe_WithoutFromKeywordsRemoved()
        {
            // Arrange
            User user = new User { Id = 1 };
            KeywordSubscriptionRequest keywordRequest =
                new KeywordSubscriptionRequest
                {
                    OperationType = OperationType.Unsubscribe,
                    Keyword = "word",
                    Source = "source"
                };
            Keyword anotherKeyword =
                new Keyword
                {
                    Word = "another",
                    Source = "local"
                };
            List<Keyword> userKeywords =
                new List<Keyword>
                {
                    new Keyword
                    {
                        Word = keywordRequest.Keyword,
                        Source = keywordRequest.Source
                    },
                    anotherKeyword
                };
            bool wasKeywordSequenceRemoved = false;

            _mockUserKeywordsRepository
                .Setup(ukr => ukr.RemoveKeywordForUser(user.Id, keywordRequest.Keyword, keywordRequest.Source))
                .Callback(() => userKeywords.RemoveAll(k => k.Word.Equals(keywordRequest.Keyword) && k.Source.Equals(keywordRequest.Source)))
                .Returns((true, true));
            _mockKeywordUpdatesProvider
                .Setup(kup => kup.RemoveKeywordSequence(keywordRequest))
                .Callback(() => wasKeywordSequenceRemoved = true);

            KeywordSubscriptionHandler keywordSubscriptionHandler =
                new KeywordSubscriptionHandler(_mockUserKeywordsRepository.Object, _mockKeywordUpdatesProvider.Object);

            IObservable<KeywordSubscriptionRequest> userKeywordRequests =
                new List<KeywordSubscriptionRequest> { keywordRequest }.ToObservable();

            // Act
            keywordSubscriptionHandler.Handle(user, userKeywordRequests).Subscribe();

            // Assert
            Assert.That(userKeywords.Count, Is.EqualTo(1));
            Assert.That(userKeywords.Any(k => k.Word.Equals(keywordRequest.Keyword) && k.Source.Equals(keywordRequest.Source)), Is.False);
            Assert.That(wasKeywordSequenceRemoved, Is.True);
        }
    }
}
