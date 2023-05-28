using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WpfApp
{
    class FileReader
    {
        public FileReader() { }
        public void ReadData(Screen screen)
        {

            List<Shape> objects = new List<Shape>();

            string filename = @"C:\Users\User\Desktop\text.csv";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open CSV File";
            dialog.Filter = "Text Files(*.txt)|*.txt|CSV Files (*.csv)|*.csv";
            if (dialog.ShowDialog() == true)
            {
                filename = dialog.FileName;
                using (var reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        switch (values[0])
                        {
                            // Triangles
                            case "HollowTriangle": objects.Add(HollowTriangle.InitializeFromCsvLine(line)); break;
                            case "FilledTriangle": objects.Add(FilledTriangle.InitializeFromCsvLine(line)); break;
                            case "GradientTriangle": objects.Add(GradientTriangle.InitializeFromCsvLine(line)); break;
                            case "PictureTriangle": objects.Add(PictureTriangle.InitializeFromCsvLine(line)); break;

                            // Lines
                            case "HorizontalLine": objects.Add(HorizontalLine.InitializeFromCsvLine(line)); break;
                            case "VerticallLine": objects.Add(VerticallLine.InitializeFromCsvLine(line)); break;
                            case "MultiLine": objects.Add(MultiLine.InitializeFromCsvLine(line)); break;

                            // Squares
                            case "HollowSquare": objects.Add(HollowSquare.InitializeFromCsvLine(line)); break;
                            case "FilledSquare": objects.Add(FilledSquare.InitializeFromCsvLine(line)); break;
                            case "GradientSquare": objects.Add(GradientSquare.InitializeFromCsvLine(line)); break;
                            case "PictureSquare": objects.Add(PictureSquare.InitializeFromCsvLine(line)); break;

                            // Circles
                            case "HollowCircle": objects.Add(HollowCircle.InitializeFromCsvLine(line)); break;
                            case "FilledCircle": objects.Add(FilledCircle.InitializeFromCsvLine(line)); break;
                            case "Gradient1Circle": objects.Add(Gradient1Circle.InitializeFromCsvLine(line)); break;
                            case "Gradient2Circle": objects.Add(Gradient2Circle.InitializeFromCsvLine(line)); break;

                            default: break;
                        }
                    }
                }
            }
            else
            {
                return;
            }

            foreach (Shape s in objects)
            {
                s.Draw(screen);
            }
        }
    }
}
