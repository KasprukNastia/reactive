using SettingsProxyAPI.Models;
using System;

namespace SettingsProxyAPI.Keywords
{
    public interface ILiveKeywordUpdatesProcessor
    {
        IObservable<KeywordOutput> AllKeywordSequencesSubject { get; }
    }
}
