using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace XamarinV14
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {



        public class Test
        {
            public string Question { get; set; }

            public List<string> Options { get; set; }

            public List<int> CorrectAnswers { get; set; }
        }

        public class TestContainer
        {
            public List<Test> Tests { get; set; }
        }

        public TestContainer tests { get; set; } = new TestContainer
        {
            Tests = new List<Test>
            {
                new Test
                {
                    Question = "Что такое жизненный цикл разработки программного обеспечения (SDLC)?",
                    Options = new List<string>
                    {
                        "Этапы, через которые проходит процесс создания программного продукта",
                        "Спецификации аппаратных требований",
                        "Маркетинговая стратегия разработки"
                    },
                    CorrectAnswers = new List<int> { 0 }
                },
                new Test
                {
                    Question = "Какие основные принципы лежат в основе гибких методологий разработки ПО (Agile)?",
                    Options = new List<string>
                    {
                        "Инкрементальность и итерационность",
                        "Жесткие рамки и строгие сроки",
                        "Отсутствие коммуникации и обратной связи"
                    },
                    CorrectAnswers = new List<int> { 0 }
                },
                new Test
                {
                    Question = "Выберите правильные утверждения о методологии Scrum:",
                    Options = new List<string>
                    {
                        "Scrum предполагает фиксированные и неразрывные итерации",
                        "Scrum включает в себя роли, такие как Product Owner и Scrum Master",
                        "Scrum идеально подходит для проектов с жесткими требованиями"
                    },
                    CorrectAnswers = new List<int> { 1 }
                },
                new Test
                {
                    Question = "Что представляет собой модель \"V\" в контексте разработки программного обеспечения?",
                    Options = new List<string>
                    {
                        "Визуализацию процесса разработки в виде буквы \"V\"",
                        "Соответствие требований и тестирование на различных этапах разработки",
                        "Название методологии разработки"
                    },
                    CorrectAnswers = new List<int> { 1 }
                },
                new Test
                {
                    Question = "Какие основные преимущества имеет применение DevOps в разработке ПО? (выберите все подходящие варианты)",
                    Options = new List<string>
                    {
                        "Улучшенное взаимодействие между разработчиками и операционной командой",
                        "Уменьшение скорости развертывания приложений",
                        "Автоматизация процессов сборки, тестирования и развертывания"
                    },
                    CorrectAnswers = new List<int> { 0, 2 }
                }
            }
        };


        private LinearLayout container;
        private Button nextButton;
        private TextView QuestionText;
        public int QuestionIndex { get; set; } = -1;
        public int RightAnswers { get; set; } = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            nextButton = FindViewById<Button>(Resource.Id.button1 );
            nextButton.Enabled = false;
            nextButton.Click += NextButton_Click;

            container = FindViewById<LinearLayout>(Resource.Id.linearLayout2);

            QuestionText = FindViewById<TextView>(Resource.Id.textView1);

            UpdateQuestion();




        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            UpdateQuestion();
        }

        public void UpdateQuestion()
        {
            CheckAnswer();
            QuestionIndex++;
            nextButton.Enabled = false;
            container.RemoveAllViews();

            if (QuestionIndex<tests.Tests.Count)
            {
                LoadQuestion(QuestionIndex);
            }
            else
            {
                QuestionText.Text = $"Правильных ответов: {RightAnswers}";
            }


        }


        public void CheckAnswer()
        {
            if (QuestionIndex < 0) return;
            var answers = anySelectedList.Cast<CompoundButton>().Select((x, i) => new { i, x }).Where(x => x.x.Checked).Select(x => x.i).OrderBy(x => x);
            var question = tests.Tests[QuestionIndex];

            if (answers.SequenceEqual(question.CorrectAnswers.OrderBy(x => x)))
            {
                RightAnswers++;
            }
        }

        private List<View> anySelectedList = null;

        public void LoadQuestion(int index)
        {
            anySelectedList = new List<View>();
            var question = tests.Tests[index];
            

            QuestionText.Text = question.Question;
            EventHandler eventHandler = delegate{
                if (anySelectedList.Any(x => (x as CompoundButton).Checked) )
                {
                    nextButton.Enabled  = true;
                }
                else
                {
                    nextButton.Enabled = false;
                }
            };


            if (question.CorrectAnswers.Count>1)
            {
                foreach (var item in question.Options)
                {
                    var checkBox = new CheckBox(this);
                    checkBox.Text = item;
                    checkBox.Click += eventHandler;
                    anySelectedList.Add(checkBox);

                    container.AddView(checkBox);
                }
            }
            else
            {
                var radioGroup = new RadioGroup(this);
                container.AddView(radioGroup);
                foreach (var item in question.Options)
                {
                    var radioButton = new RadioButton(this);
                    radioButton.Text = item;

                    radioButton.Click += eventHandler;
                    anySelectedList.Add(radioButton);

                    radioGroup.AddView(radioButton);
                }


            }


        }

        

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }













	}
}
