using Content.Client.UserInterface.Controls;
using Content.Shared._Impstation.Thaven;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Utility;

namespace Content.Client._Impstation.Thaven.Eui;

[GenerateTypedNameReferences]
public sealed partial class ThavenMoodUi : FancyWindow
{
    private List<ThavenMood> _moods = new();

    public ThavenMoodUi()
    {
        RobustXamlLoader.Load(this);
        NewMoodButton.OnPressed += _ => AddNewMood();
    }

    private void AddNewMood()
    {
        MoodContainer.AddChild(new MoodContainer());
    }

    public List<ThavenMood> GetMoods()
    {
        var newMoods = new List<ThavenMood>();

        foreach (var control in MoodContainer.Children)
        {
            if (control is not MoodContainer moodControl)
                continue;

            if (string.IsNullOrWhiteSpace(moodControl.ThavenMoodTitle.Text))
                continue;

            var moodText = Rope.Collapse(moodControl.ThavenMoodContent.TextRope).Trim();

            if (string.IsNullOrWhiteSpace(moodText))
                continue;

            var mood = new ThavenMood()
            {
                MoodName = moodControl.ThavenMoodTitle.Text,
                MoodDesc = moodText,
            };

            newMoods.Add(mood);
        }

        return newMoods;
    }

    private void MoveUp(int index)
    {
        if (index <= 0)
            return;

        (_moods[index], _moods[index - 1]) = (_moods[index - 1], _moods[index]);
        SetMoods(_moods);
    }

    private void MoveDown(int index)
    {
        if (index >= _moods.Count - 1)
            return;

        (_moods[index], _moods[index + 1]) = (_moods[index + 1], _moods[index]);
        SetMoods(_moods);
    }

    private void Delete(int index)
    {
        _moods.RemoveAt(index);

        SetMoods(_moods);
    }

    public void SetMoods(List<ThavenMood> moods)
    {
        _moods = moods;
        MoodContainer.RemoveAllChildren();
        for (var i = 0; i < moods.Count; i++)
        {
            var index = i; // Copy for the closure
            var mood = moods[i];
            var moodControl = new MoodContainer(mood);
            moodControl.MoveUp.OnPressed += _ => MoveUp(index);
            moodControl.MoveDown.OnPressed += _ => MoveDown(index);
            moodControl.Delete.OnPressed += _ => Delete(index);
            MoodContainer.AddChild(moodControl);
        }
    }
}
