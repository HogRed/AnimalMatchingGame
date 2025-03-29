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
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }

}
