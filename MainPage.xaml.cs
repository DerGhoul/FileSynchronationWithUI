using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace FileSynchronationWithUI
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            mainMethod();
        }
        static List<string> fileList = new List<string>();
        public void mainMethod() {
            ///Variablendeklaration
            string fileName;
            string destFile;
            string zipFileName;
            string quellOrdner;
            string zielOrdner;
            string createdfileName;
            string[] folderNames = new string[2];
            bool sourceFolderExists = false;
            string destinationFolderName;
            ///Eingabeabfrage            
            folderNames = folderNameInteraction();
            quellOrdner = folderNames[0];
            zielOrdner = folderNames[1];

            ///Kreiert Ordnername
            createdfileName = createFileName();

            ///Erstellt Ordner in zielOrdner
            destinationFolderName = zielOrdner + createdfileName;
            System.IO.Directory.CreateDirectory(destinationFolderName);

            ///Überprüft ob Quellordner Existiert
            if (System.IO.Directory.Exists(quellOrdner))
            {
                sourceFolderExists = true;
            }

            ///Einlesen der Dateien
            List<string> files = getFiles(quellOrdner);
            string fileString = "";

            if (sourceFolderExists == true)
            {
                foreach (var file in files)
                {
                    fileString += (file + "\n");

                    fileName = System.IO.Path.GetFileName(file);
                    destFile = System.IO.Path.Combine(destinationFolderName, fileName);
                    System.IO.File.Copy(file, destFile, true);
                }
            }


            zipFileName = (destinationFolderName + ".zip");

            if (System.IO.Directory.Exists(destinationFolderName))
            {
                ZipFile.CreateFromDirectory(destinationFolderName, zipFileName);
            }
        }

        public static List<string> getFiles(string folderName)
        {

            string[] files = System.IO.Directory.GetFiles(folderName);
            string[] folders = System.IO.Directory.GetDirectories(folderName);
            foreach (var folder in folders)
            {
                //Console.WriteLine(folder);
                getFiles(folder);
            }

            foreach (var file in files)
            {
                //Console.WriteLine(file);
                fileList.Add(file);
            }
            return fileList;
        }

        public static string createFileName()
        {
            //Variablendeklaration
            string dateTimeNow = DateTime.Now.ToString();
            string[] datTimeNowToStringWithoutPoints = new string[3];
            string[] timeWithoutDoublePoints = new string[3];
            string[] dateTimeNowTime = new string[2];
            string createdfileName;


            //Console.WriteLine(dateTimeNow);
            //Methodenkörper
            dateTimeNowTime = dateTimeNow.Split(' ');
            datTimeNowToStringWithoutPoints = dateTimeNowTime[0].Split('.');
            timeWithoutDoublePoints = dateTimeNowTime[1].Split(':');
            dateTimeNow = datTimeNowToStringWithoutPoints[0] + datTimeNowToStringWithoutPoints[1] + datTimeNowToStringWithoutPoints[2] + "_" +
                            timeWithoutDoublePoints[0] + timeWithoutDoublePoints[1] + timeWithoutDoublePoints[2];


            ///Dateipfade werden hier eingeschrieben
            createdfileName = $@"\FileSync{dateTimeNow}";


            return createdfileName;
        }

        public static string[] folderNameInteraction()
        {

            string[] folderNames = new string[2];
            string quellOrdner;
            string zielOrdner;
            /*
            Console.WriteLine("Gib einen Quellordner an");
            quellOrdner = Console.ReadLine();
            Console.WriteLine("Wenn sie fortfahren wollen drücken sie bitte eine beliebige Taste");
            Console.ReadKey();
            Console.WriteLine("Geben sie einen Zielordner an");
            zielOrdner = Console.ReadLine();
            Console.WriteLine("Drücken sie eine beliebige Taste zum starten.");
            Console.ReadKey();
            */
            folderNames[0] = quellOrdner;
            folderNames[1] = zielOrdner;
            return folderNames;
        }
    }

}
