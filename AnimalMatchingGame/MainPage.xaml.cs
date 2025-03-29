namespace AnimalMatchingGame
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void PlayAgainButton_Clicked(object sender, EventArgs e)
        {
            AnimalButtons.IsVisible = true; // make animal buttons visible
            PlayAgainButton.IsVisible = false; // make play again button invisible

            // list of animal emoji pairs
            // used Win + ; key for native emoji keyboard
            List<string> animalEmoji = [
                "🐫","🐫",
                "🐮","🐮",
                "🐶","🐶",
                "🐨","🐨",
                "🐼","🐼",
                "🐲","🐲",
                "🐏","🐏",
                "🐗","🐗",
                ];

            // iterate over each button in AnimalButtons,
            // randomly assign it an animal emoji, and remove
            // that emoji from the list so it's not reused.
            foreach (var button in AnimalButtons.Children.OfType<Button>())
            {
                int index = Random.Shared.Next(animalEmoji.Count);
                string nextEmoji = animalEmoji[index];
                button.Text = nextEmoji;
                animalEmoji.RemoveAt(index);
            }

            // start a timer that execute TimerTick every 0.1 seconds
            Dispatcher.StartTimer(TimeSpan.FromSeconds(.1), TimerTick);
        }

        int tenthsOfSecondsElapsed = 0; // keep track of elapsed time in tenths of seconds

        private bool TimerTick()
        {
            if(!this.IsLoaded) return false; // stop the timer if the page is not loaded

            tenthsOfSecondsElapsed++; // increment the elapsed time

            TimeElapsed.Text = "Time Elapsed: " +
                (tenthsOfSecondsElapsed / 10F).ToString("0.0s"); // update the label with elapsed time

            if (PlayAgainButton.IsVisible) // check if the play again button is visible
            {
                tenthsOfSecondsElapsed = 0; // reset the elapsed time
                return false; // stop the timer
            }

            return true; // continue the timer
        }

        // variables to keep track of button matches
        Button lastClicked;
        bool findingMatch = false;
        int matchesFound;

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (sender is Button buttonClicked)
            {
                if (!string.IsNullOrWhiteSpace(buttonClicked.Text) && (findingMatch == false))
                {
                    buttonClicked.BackgroundColor = Colors.Red; // change color to red when clicked
                    lastClicked = buttonClicked; // store the last clicked button
                    findingMatch = true; // we're now looking for a match
                }

                else
                {
                    if ((buttonClicked != lastClicked) && (buttonClicked.Text == lastClicked.Text) && (!String.IsNullOrWhiteSpace(buttonClicked.Text)))
                    {
                        matchesFound++;
                        lastClicked.Text = " "; // clear the text of the matched button
                        buttonClicked.Text = " "; // clear the text of the current button
                    }

                    lastClicked.BackgroundColor = Colors.Blue; // reset the color of the last clicked button
                    buttonClicked.BackgroundColor = Colors.Blue; // reset the color of the current button
                    findingMatch = false; // no longer looking for a match
                }
            }

            if (matchesFound == 8) // check if all matches are found
            {
                matchesFound = 0; // reset matches found for next game
                AnimalButtons.IsVisible = false; // hide animal buttons
                PlayAgainButton.IsVisible = true; // show play again button
            }
        }
    }
}
