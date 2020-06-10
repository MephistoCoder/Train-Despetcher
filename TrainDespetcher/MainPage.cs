using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TrainDespetcher
{
    public partial class MainPage : Form
    {
        SQLConnector sqlConnector = new SQLConnector();
        List<Train> array = new List<Train>();
        Train currTrain;
        
        public MainPage()
        {
            InitializeComponent();
            
            
            
            sqlConnector.selectAll();

            StartPage startPage = new StartPage();
            startPage.ShowDialog();
            
            
            listBox1.Items.AddRange(sqlConnector.arr);
            foreach(Train a in sqlConnector.arr)
            {
                comboBox1.Items.Add(a.City);
            }
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
           
        }

        
        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPage addPage = new AddPage(sqlConnector, currTrain);
            addPage.ShowDialog();
        }

        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPage renamePage = new AddPage(sqlConnector, currTrain);
            if (currTrain == null)
            {
                MessageBox.Show("Для редагування рейсу, оберіть рейс який ви хочете відредагувати");

            }
            else
            {

                renamePage.Text = "Редагування рейсу";
                renamePage.textBox1.Text = currTrain.Number;
                renamePage.textBox2.Text = currTrain.City;
                renamePage.textBox3.Text = Convert.ToString(currTrain.Tickets);
                renamePage.maskedTextBox1.Text = currTrain.StartTime;
                renamePage.textBox4.Text = currTrain.WayTime;
                renamePage.ShowDialog();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            if(textBox1.TextLength != 0)
            {
                Train[] trains = new Train[listBox1.Items.Count];
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    trains[i] = (Train)listBox1.Items[i];
                }
                if(Train.searchTrainByNumber(trains, textBox1.Text) != null)
                {
                    listBox3.Items.Add("Квитків: " + Train.searchTrainByNumber(trains, textBox1.Text).Tickets);
                    
                }
                               
            } else
            {
                MessageBox.Show("Не введено номер потягу", "Некоректний ввод");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            currTrain = (Train) listBox1.SelectedItem;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            
            if (!maskedTextBox1.MaskCompleted)
            {
                MessageBox.Show("Не введено час від якого необхідно почати пошук", "Некоректні дані");
            } else
            {
                
                 if (!maskedTextBox2.MaskCompleted)
                {
                    MessageBox.Show("Не введено час до якого необхідно вести пошук", "Некоректні дані");
                }
                else
                {
                    Train[] trains = new Train[listBox1.Items.Count];
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        trains[i] = (Train)listBox1.Items[i];
                    }
                    List<Train> arrayList = Train.searchFromAtoB(trains, maskedTextBox1.Text, maskedTextBox2.Text,
                        comboBox1.Text);
                    if(arrayList.Count != 0)
                    {
                        Train[] trainsSearch = (Train[])arrayList.ToArray();
                        listBox2.Items.AddRange(trainsSearch);
                    }
                    else
                    {
                        MessageBox.Show("Рейси за заданими параметрами не знайдені", "Збіжності не знайдені");
                    }
                }

            }

            
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count != 0 && listBox3.Items.Count != 0)
            {


                Train[] trains = new Train[listBox1.Items.Count];
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    trains[i] = (Train)listBox1.Items[i];
                }
                Microsoft.Office.Interop.Word.Application application;
                Microsoft.Office.Interop.Word.Document document;
                application = new Microsoft.Office.Interop.Word.Application();
                string filePath = Application.StartupPath;
                document = application.Documents.Add(filePath + "\\Data.docx");
                Object missing = Type.Missing;
                Microsoft.Office.Interop.Word.Range selText;
                selText = document.Range(document.Content.Start, document.Content.End);
                Microsoft.Office.Interop.Word.Find find = application.Selection.Find;

                find.Text = "[Number]";
                find.Replacement.Text = Convert.ToString(comboBox1.Items[comboBox1.SelectedIndex]);

                Object wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
                Object replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
                find.Execute(FindText: Type.Missing,
                MatchCase: false,
                MatchWholeWord: false,
                MatchWildcards: false,
                MatchSoundsLike: missing,
                MatchAllWordForms: false,
                Forward: true,
                Wrap: wrap,
                Format: false,
                ReplaceWith: missing, Replace: replace);


                for (int rowNum = 0; rowNum < listBox3.Items.Count; rowNum++)
                {
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        if (trains[i].Number == textBox1.Text)
                        {
                            document.Tables[1].Cell(2 + rowNum, 1).Range.Text = trains[i].Number;
                            document.Tables[1].Cell(2 + rowNum, 2).Range.Text = Convert.ToString(trains[i].Tickets);
                            if (listBox3.Items.Count > 1)
                            {
                                document.Tables[1].Rows.Add();
                            }
                        }
                    }
                }
                find.Text = "[City]";
                find.Replacement.Text = Convert.ToString(comboBox1.Items[comboBox1.SelectedIndex]);

                find.Execute(FindText: Type.Missing,
                MatchCase: false,
                MatchWholeWord: false,
                MatchWildcards: false,
                MatchSoundsLike: missing,
                MatchAllWordForms: false,
                Forward: true,
                Wrap: wrap,
                Format: false,
                ReplaceWith: missing, Replace: replace);

                find.Text = "[start]";
                find.Replacement.Text = Convert.ToString(maskedTextBox1.Text);

                find.Execute(FindText: Type.Missing,
                MatchCase: false,
                MatchWholeWord: false,
                MatchWildcards: false,
                MatchSoundsLike: missing,
                MatchAllWordForms: false,
                Forward: true,
                Wrap: wrap,
                Format: false,
                ReplaceWith: missing, Replace: replace);

                find.Text = "[end]";
                find.Replacement.Text = Convert.ToString(maskedTextBox2.Text);

                find.Execute(FindText: Type.Missing,
                MatchCase: false,
                MatchWholeWord: false,
                MatchWildcards: false,
                MatchSoundsLike: missing,
                MatchAllWordForms: false,
                Forward: true,
                Wrap: wrap,
                Format: false,
                ReplaceWith: missing, Replace: replace);

                for (int rowNum = 0; rowNum < listBox2.Items.Count; rowNum++)
                {
                    string fStr = listBox2.Items[rowNum].ToString();
                    int charNum1;
                    charNum1 = fStr.IndexOf(" ");
                    fStr = fStr.Substring(0, charNum1);
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        if (trains[i].Number == fStr)
                        {
                            document.Tables[2].Cell(2 + rowNum, 1).Range.Text = trains[i].Number;
                            document.Tables[2].Cell(2 + rowNum, 2).Range.Text = trains[i].City;
                            document.Tables[2].Cell(2 + rowNum, 3).Range.Text = trains[i].StartTime;
                            if (listBox2.Items.Count > 1)
                            {
                                document.Tables[2].Rows.Add();
                            }
                        }
                    }
                }

                document.Save();
                document.Close();
                application.Quit();
                MessageBox.Show("Сбереження відбулося успішно", "Довідка");

            }
            else
            {
                MessageBox.Show("Необхідно виконати обидва пошуки, щоб занести дані до файлу", "Довідка");
            }
        }

        private async void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await sqlConnector.selectAll();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(sqlConnector.arr);
        }

        private void MainPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            sqlConnector.exit();
        }
    }
}
