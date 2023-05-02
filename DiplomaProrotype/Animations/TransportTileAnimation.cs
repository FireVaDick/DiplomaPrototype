using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace DiplomaPrototype.Animations
{
    internal class TransportTileAnimation
    {
        private List<Storyboard> storyboards; // список Storyboard

        private int currentStoryboardIndex = 0; // индекс текущего Storyboard

        public TransportTileAnimation(List<Storyboard> storyboards)
        {
            this.storyboards = storyboards;
        }

        public void StartAnimation()
        {
            // Запускаем первый Storyboard
            storyboards[currentStoryboardIndex].Begin();
            // Подписываемся на событие Completed
            storyboards[currentStoryboardIndex].Completed += StoryboardCompleted;
        }

        private void StoryboardCompleted(object sender, EventArgs e)
        {
            // Отписываемся от события Completed
            storyboards[currentStoryboardIndex].Completed -= StoryboardCompleted;
            // Увеличиваем индекс текущего Storyboard
            currentStoryboardIndex++;
            // Если мы дошли до конца списка, то начинаем сначала
            if (currentStoryboardIndex >= storyboards.Count)
            {
                currentStoryboardIndex = 0;
            }
            // Запускаем следующий Storyboard
            storyboards[currentStoryboardIndex].Begin();
            // Подписываемся на событие Completed
            storyboards[currentStoryboardIndex].Completed += StoryboardCompleted;
        }

    }
}
