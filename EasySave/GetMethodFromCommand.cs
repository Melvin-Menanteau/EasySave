using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EasySave
{
    public class GetMethodFromCommand
    {
        public void GetMethod(string command)
        {
            if (command.StartsWith("ls"))
            {
                List<int> IdsSauvegarde = new List<int>();

                if (command.Contains("-n"))
                {
                    string NomSauvegarde = GetNomFromCommand(command);

                    if (NomSauvegarde == null)
                    {
                        Console.WriteLine("Erreur - Commande LS non reconnue");
                    }

                    int IdSauvegarde = GetIdFromNom(NomSauvegarde);

                    if (IdSauvegarde > 0)
                    {
                        IdsSauvegarde.Add(IdSauvegarde);
                    }
                    else
                    {
                        Console.WriteLine("Erreur - Le nom de la sauvegarde n'existe pas ");
                    }
                }
                else if (command.Contains("-id"))
                {
                    IdsSauvegarde = GetIdsFromCommand(command);
                }
                else if (command.Contains("-all"))
                {
                    IdsSauvegarde = GetAllIdsFromCommand();
                }

                if (IdsSauvegarde.Count == 0)
                {
                    Console.WriteLine("Erreur - Commande LS non reconnue");
                }
            }
        }

        public List<int> GetAllIdsFromCommand()
        {
            List<int> Ids = new List<int>();

            //Simulation de la liste des sauvegarde existantes
            Dictionary<string, int> ids = new Dictionary<string, int>
            {
                { "nom1", 1 },
                { "nom2", 2 },
                { "nom3", 3 },
                { "nom4", 4 },
                { "nom5", 5 }
            };

            foreach (var elem in ids)
            {
                Ids.Add(elem.Value);
            }

            return Ids;
        }

        public string GetNomFromCommand(string command)
        {
            string nom = "";

            Regex regex = new Regex(@"-n\s+""([^""]*)""");
            Match match = regex.Match(command);

            if (match.Success)
            {
                nom = match.Groups[1].Value;
            }
            else if (command.Contains("-n"))
            {
                Console.WriteLine("Erreur - Regex non conforme");
                nom = null;
            }

            return nom;
        }

        public int GetIdFromNom(string nom)
        {
            int Id;

            //Simulation de la liste des sauvegarde existantes
            Dictionary<string, int> ids = new Dictionary<string, int>
            {
                { "nom1", 1 },
                { "nom2", 2 },
                { "nom3", 3 },
                { "nom4", 4 },
                { "nom5", 5 }
            };

            if (ids.TryGetValue(nom, out int valeur))
            {
                Id = valeur;
            }
            else
            {
                Id = -1;
            }

            return Id;
        }

        public List<int> GetIdsFromCommand(string command)
        {
            List<int> Ids = new List<int>();
            string IdsString = "";

            //Simulation de la liste des sauvegarde existantes
            List<int> idsSauvegardeExistant = new List<int> { 1, 2, 3, 4, 5 };

            Regex regex = new Regex(@"-id\s+""([^""]*)""");
            Match match = regex.Match(command);

            if (match.Success)
            {
                IdsString = match.Groups[1].Value;


                if(IdsString.Contains("-") && IdsString.Contains(";"))
                {
                    Console.WriteLine("Erreur - La commande LS ne peut pas contenir - et ;");
                }
                else if (IdsString.Contains("-"))
                {
                    string[] Id = IdsString.Split('-');

                    int StartId = int.Parse(Id[0]);
                    int EndId = int.Parse(Id[1]);

                    if (StartId > EndId)
                    {
                        (EndId, StartId) = (StartId, EndId);
                    }

                    foreach (int id in idsSauvegardeExistant)
                    {
                        if (StartId <= id && id <= EndId)
                        {
                            Ids.Add(id);
                        }
                    }
                }
                else if (IdsString.Contains(";"))
                {
                    string[] IdString = IdsString.Split(';');
                    int[] IdInt = new int[IdString.Length];
                    try
                    {
                        IdInt = IdString.Select(int.Parse).ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    foreach (int id in IdInt)
                    {
                        if (idsSauvegardeExistant.Contains(id))
                        {
                            Ids.Add(id);
                        }
                    }
                }
                else if (int.TryParse(IdsString, out int id))
                {
                    Ids.Add(id);
                }
            }
            else
            {
                Console.WriteLine("Erreur - Regex non conforme");
            }

            return Ids;
        }
    }
}
