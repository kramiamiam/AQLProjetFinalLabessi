using System;
using System.Collections.Generic;
using System.IO;

namespace AQLProjetFinalLabessiOmarAkram
{
    class Program
    {
        static void Main(string[] args)
        {
            // Listes pour stocker les etudiants les cours et les notes
            var etudiants = new List<Etudiant>();
            var cours = new List<Cours>();
            var notes = new List<Note>();

            while (true)
            {
                // Affichage du menu 
                Console.WriteLine("Que voulez-vous faire :\n- Ajouter un étudiant: Tapez 1\n- Ajouter un cours: Tapez 2\n- Ajouter une note: Tapez 3\n- Consulter les notes d'un étudiant: Tapez 4\n- Quitter: Tapez 5");

                // Lecture du choix de l'utilisateur
                switch (Console.ReadLine())
                {
                    case "1":
                        // Ajout d'un etudiant
                        AjouterElement(etudiants, "étudiant", e => new Etudiant(e.Numero, e.Nom, e.Prenom));
                        break;
                    case "2":
                        // Ajout d'un cours
                        AjouterElement(cours, "cours", c => new Cours(c.Numero, c.Code, c.Titre));
                        break;
                    case "3":
                        // Ajout d'une note
                        AjouterNote(etudiants, cours, notes);
                        break;
                    case "4":
                        // Consultation des notes 
                        ConsulterNotes(etudiants, cours, notes);
                        break;
                    case "5":
                        // Sauvegarde des donnees et fin 
                        Sauvegarder("etudiants.txt", etudiants);
                        Sauvegarder("cours.txt", cours);
                        Sauvegarder("notes.txt", notes);
                        Console.WriteLine("SUCCES!");
                        return;
                    default:
                        Console.WriteLine("CHOIX INVALIDE, VEUILLEZ REESSAYER!");
                        break;
                }
            }
        }

        // Methode pour ajouter un element 
        static void AjouterElement<T>(List<T> list, string type, Func<dynamic, T> createElement)
        {
            try
            {
                dynamic input = new System.Dynamic.ExpandoObject();

                Console.WriteLine($"Entrez le numéro du {type} :");
                input.Numero = int.Parse(Console.ReadLine());

                if (typeof(T) == typeof(Etudiant))
                {
                    Console.WriteLine("Entrez le nom de l'étudiant :");
                    input.Nom = Console.ReadLine();
                    Console.WriteLine("Entrez le prénom de l'étudiant :");
                    input.Prenom = Console.ReadLine();
                }
                else if (typeof(T) == typeof(Cours))
                {
                    Console.WriteLine("Entrez le code du cours :");
                    input.Code = Console.ReadLine();
                    Console.WriteLine("Entrez le titre du cours :");
                    input.Titre = Console.ReadLine();
                }

                // Creation et ajout de l'element à la liste
                list.Add(createElement(input));
                Console.WriteLine($"{type.Capitalize()} ajouté!");
            }
            catch (FormatException)
            {
                Console.WriteLine("INVALIDE. VEUILLEZ REESSAYER.");
            }
        }

        // Methode pour ajouter une note
        static void AjouterNote(List<Etudiant> etudiants, List<Cours> cours, List<Note> notes)
        {
            try
            {
                Console.WriteLine("Entrez le numéro de l'étudiant :");
                int numEtudiant = int.Parse(Console.ReadLine());

                // Verification si l'etudiant existe
                if (etudiants.Find(e => e.Numero == numEtudiant) == null)
                {
                    Console.WriteLine("Étudiant inexistant.");
                    return;
                }

                Console.WriteLine("Entrez le numéro du cours :");
                int numCours = int.Parse(Console.ReadLine());

                // Verification si le cours existe
                if (cours.Find(c => c.Numero == numCours) == null)
                {
                    Console.WriteLine("Cours inexistant.");
                    return;
                }

                Console.WriteLine("Entrez la note :");
                notes.Add(new Note(numEtudiant, numCours, double.Parse(Console.ReadLine())));
                Console.WriteLine("Note ajoutée.");
            }
            catch (FormatException)
            {
                Console.WriteLine("INVALIDE. VEUILLEZ REESSAYEZ");
            }
        }

        // Methode pour consulter les notes d'un etudiant
        static void ConsulterNotes(List<Etudiant> etudiants, List<Cours> cours, List<Note> notes)
        {
            try
            {
                Console.WriteLine("Entrez le numéro de l'étudiant :");
                int numEtudiant = int.Parse(Console.ReadLine());

                var etudiant = etudiants.Find(e => e.Numero == numEtudiant);
                if (etudiant == null)
                {
                    Console.WriteLine("Étudiant inexistant.");
                    return;
                }

                Console.WriteLine($"Relevé de notes de {etudiant.Prenom} {etudiant.Nom} :");
                foreach (var note in notes.FindAll(n => n.NumEtudiant == numEtudiant))
                {
                    var coursInfo = cours.Find(c => c.Numero == note.NumCours);
                    Console.WriteLine($"Cours: {coursInfo.Titre}({coursInfo.Code}) - Note: {note.Valeur}");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("INVALIDE. VEUILLEZ REESSAYER");
            }
        }

        // Methode pour sauvegarder une liste d'elements dans un fichier
        static void Sauvegarder<T>(string filename, List<T> list)
        {
            using (var sw = new StreamWriter(filename))
            {
                foreach (var item in list)
                {
                    sw.WriteLine(item);
                }
            }
        }
    }

    // Classe Etudiant
    class Etudiant
    {
        public int Numero { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }

        public Etudiant(int numero, string nom, string prenom) => (Numero, Nom, Prenom) = (numero, nom, prenom);
        public override string ToString() => $"{Numero},{Nom},{Prenom}";
    }

    // Classe Cours
    class Cours
    {
        public int Numero { get; set; }
        public string Code { get; set; }
        public string Titre { get; set; }

        public Cours(int numero, string code, string titre) => (Numero, Code, Titre) = (numero, code, titre);
        public override string ToString() => $"{Numero},{Code},{Titre}";
    }

    // Classe Note
    class Note
    {
        public int NumEtudiant { get; set; }
        public int NumCours { get; set; }
        public double Valeur { get; set; }

        public Note(int numEtudiant, int numCours, double valeur) => (NumEtudiant, NumCours, Valeur) = (numEtudiant, numCours, valeur);
        public override string ToString() => $"{NumEtudiant},{NumCours},{Valeur}";
    }

    // Methode pour mettre la majuscule au premier caractere d'une chaine
    static class Extensions
    {
        public static string Capitalize(this string str) => char.ToUpper(str[0]) + str.Substring(1);
    }
}