namespace AnimalMatchingGame
{
    public partial class MainPage : ContentPage
    {
        int totalTenthsOfSeconds = 300; // total time in tenths of seconds (30 seconds)

        public MainPage()
        {
            InitializeComponent();
        }

        private void PlayAgainButton_Clicked(object sender, EventArgs e)
        {
            AnimalButtons.IsVisible = true; // make animal buttons visible
            PlayAgainButton.IsVisible = false; // make play again button invisible

            // list of animal emoji pairs
            List<string> animalEmoji = new List<string>
                        {
                            "🐫","🐮","🐶","🐨","🐼","🐲","🐏","🐗",
                            "🐵","🦓","🐅","🐔","🦒","🦦","🦈","🐬",
                            "🦐","🦑","🦞","🦀"
                        };

            // select 8 unique emojis, generate pairs, shuffle, and assign to buttons
            var selectedEmojis = animalEmoji.OrderBy(a => Guid.NewGuid()).Take(8).ToList();
            var emojiPairs = selectedEmojis.SelectMany(e => new List<string> { e, e }).OrderBy(a => Guid.NewGuid()).ToList();

            // iterate over each button in AnimalButtons,
            // assign it an animal emoji from the shuffled list
            int i = 0;
            foreach (var button in AnimalButtons.Children.OfType<Button>())
            {
                button.Text = emojiPairs[i];
                i++;
            }

            // reset the timer
            tenthsOfSecondsElapsed = totalTenthsOfSeconds;

            // start a timer that execute TimerTick every 0.1 seconds
            Dispatcher.StartTimer(TimeSpan.FromSeconds(.1), TimerTick);
        }

        int tenthsOfSecondsElapsed; // keep track of remaining time in tenths of seconds

        private bool TimerTick()
        {
            if (!this.IsLoaded) return false; // stop the timer if the page is not loaded

            tenthsOfSecondsElapsed--; // decrement the remaining time

            TimeElapsed.Text = "Time Remaining: " +
                ((tenthsOfSecondsElapsed) / 10F).ToString("0.0s"); // update the label with remaining time
                                                                       // sub one to account for extra tick at game conclusion

            if (tenthsOfSecondsElapsed <= 0) // check if time has run out
            {
                // end the game
                AnimalButtons.IsVisible = false; // hide animal buttons
                PlayAgainButton.IsVisible = true; // show play again button
                return false; // stop the timer
            }

            if (PlayAgainButton.IsVisible) // check if the play again button is visible
            {
                tenthsOfSecondsElapsed = totalTenthsOfSeconds; // reset the elapsed time
                return false; // stop the timer
            }

            return true; // continue the timer
        }

        // variables to keep track of button matches
        Button lastClicked;
        bool findingMatch = false;
        int matchesFound;

        // keep track of best time
        private float bestTime = float.MaxValue;

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
                // Calculate elapsed time in seconds
                float currentTime = ((totalTenthsOfSeconds - tenthsOfSecondsElapsed) + 1) / 10F; // +1 to account for the extra tick at the end

                // Update current time
                CurrentTime.Text = "Current Time: " + currentTime.ToString("0.0s");

                // Update bestTime if the current time is better (i.e., lower)
                if (currentTime < bestTime)
                {
                    bestTime = currentTime;
                    BestTime.Text = "Best Time: " + bestTime.ToString("0.0s");
                }

                matchesFound = 0; // reset for the next game
                AnimalButtons.IsVisible = false; // hide animal buttons
                PlayAgainButton.IsVisible = true; // show play again button
            }
        }
    }
}
