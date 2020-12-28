using Moq;
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
    class KeywordUpdatesProviderTest
    {
        private Mock<IKeywordInfoRepository> _mockKeywordInfoRepository;
        private Mock<ILiveKeywordUpdatesProcessor> _mockLiveKeywordUpdatesProcessor;

        [SetUp]
        public void SetUp()
        {
            _mockKeywordInfoRepository = new Mock<IKeywordInfoRepository>();
            _mockLiveKeywordUpdatesProcessor = new Mock<ILiveKeywordUpdatesProcessor>();
        }

        [Test]
        public void GetMergedSequence()
        {
            KeywordRequest keywordRequest = new KeywordRequest { Keyword = "word", Source = "source" };
            List<KeywordOutput> liveKeywords =
                new List<KeywordOutput>
                {
                    new KeywordOutput
                    {
                        Keyword = keywordRequest.Keyword,
                        Source = keywordRequest.Source
                    },
                    new KeywordOutput
                    {
                        Keyword = "another",
                        Source = "local"
                    }
                };
            List<KeywordInfo> unprocessedKeywords =
                new List<KeywordInfo>
                {
                    new KeywordInfo
                    {
                        Word = keywordRequest.Keyword,
                        Source = keywordRequest.Source
                    }
                };

            _mockLiveKeywordUpdatesProcessor
                .Setup(kup => kup.AllKeywordSequencesSubject)
                .Returns(liveKeywords.ToObservable());
            _mockKeywordInfoRepository
                .Setup(kir => kir.GetAllUnprocessedRecords(keywordRequest.Keyword, keywordRequest.Source))
                .Returns(unprocessedKeywords.ToAsyncEnumerable());

            KeywordUpdatesProvider keywordUpdatesProvider =
                new KeywordUpdatesProvider(_mockKeywordInfoRepository.Object, _mockLiveKeywordUpdatesProcessor.Object);

            // Act, Assert
            keywordUpdatesProvider.GetKeywordSequence(keywordRequest)
                .Do(onNext: keywordOutput =>
                {
                    Assert.That(keywordOutput.Keyword.Equals(keywordRequest.Keyword));
                    Assert.That(keywordOutput.Source.Equals(keywordRequest.Source));
                    Assert.That(keywordUpdatesProvider.ObservedKeywordsCount, Is.EqualTo(1));
                })
                .Subscribe();
        }
    }
}
