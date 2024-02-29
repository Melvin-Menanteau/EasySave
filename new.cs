using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

enum errorReception : int
{
    AbsenceReponse = -1,
    DepassementAbsenceReponseMax = -2
}

class GameFindNumber
{
    public static int nbJoueur;//nb joueur connecté
    private int secretNumber; // numéro secret à deviner
    private string pseudo; // pseudo du joueur
    private string AdresseIPclient; //adresse IP du joueur
    private int portIpclient; // port ip du joueur
    Socket clientSocket;
    int nbNonReponse = 0; // compteur de nonRéponse

    public GameFindNumber()
    {
        nbJoueur++;
        secretNumber = 0;
        nbNonReponse = 0;
        pseudo = "Player" + nbJoueur.ToString();
    }

    // les informations sont affichées dans la console et enregistrées dans le fichier log
    private bool LogInformation(string message)
    {
        string ligneLog;
        try
        {
            ligneLog = $"{DateTime.Now};{AdresseIPclient};{portIpclient};{pseudo};{secretNumber};{message}";
            LoggerFile.Instance.Log(ligneLog);
            Console.WriteLine(ligneLog);
            return true;
        }
        catch
        {
            return false;
        }

    }
    //fonction générant un nombre aléatoire entre 1 et 100
    public int InitializeGame()
    {
        Random random = new Random();
        this.secretNumber = random.Next(1, 101);
        LogInformation($"Nombre secret généré : {this.secretNumber}");
        return this.secretNumber;
    }

    //envoie d'un message au client IP
    public bool SendMessage(string msg)
    {
        try
        {
            this.clientSocket.Send(Encoding.ASCII.GetBytes($"{msg}"));
            LogInformation("message envoyé :" + msg);
        }
        catch
        {
            LogInformation("Impossible d'envoyer :" + msg);
            return false;
        }
        return true;
    }

    // recupération du pseudo du joueur
    public bool RecuperationPseudo()
    {
        while (true)
        {
            SendMessage("Quel est votre pseudo ?");
            var responseReceived = ReceiveDataWithTimeout(20000);
            if (responseReceived.Item1 > 0) //reception du pseudo
            {
                this.pseudo = responseReceived.Item2;
                return true;

            }
            //dépassement du nombre de requètes sans réponse
            if (responseReceived.Item1 == (int)errorReception.DepassementAbsenceReponseMax)
            {
                return false;
            }
            //pas de réponse du client IP
            if (responseReceived.Item1 == (int)errorReception.AbsenceReponse)
            {
                LogInformation($"Pas de réponse du client {nbNonReponse}.");
            }
        }
    }

    // thread principal gérant la connexion
    public void Game(object clientObj)
    {
        bool gameOver = false;
        int nbNonReponse = 0;
        this.clientSocket = (Socket)clientObj;
        /* TODO
         * Renseigner AdresseIPclient
         * Renseigner PortIPClient
         */
        AdresseIPclient = ";";
        portIpclient = 0;


        try
        {

            if (RecuperationPseudo()) // on doit récupérer un pseudo pour jouer
            {
                InitializeGame(); // génération du nombre aléatoire
                LogInformation($"Le joueur vient de se connecter. Il doit deviner le nombre {this.secretNumber}");
                while (!gameOver) // on boucle tant que le joueur n'a pas la réponse
                {
                    SendMessage($"{this.pseudo}, Veuillez proposer un nombre : ");
                    var responseReceived = ReceiveDataWithTimeout(10000);
                    //dépassement du nombre de requètes sans réponse Timeout
                    if (responseReceived.Item1 == (int)errorReception.DepassementAbsenceReponseMax) break;
                    //requètes sans réponse Timeout
                    if (responseReceived.Item1 == (int)errorReception.AbsenceReponse)
                    {
                        LogInformation($"Pas de réponse du client {nbNonReponse}. Relance de la demande.");
                    }
                    else // analyse de la réponse
                    {
                        int guess;
                        if (int.TryParse(responseReceived.Item2, out guess))
                        {
                            LoggerFile.Instance.Log($"{pseudo} a proposé : {guess}");
                            if (AnalyseNumber(guess)) gameOver = true;
                        }
                        else //la réponse n'est pas un entier
                        {
                            SendMessage("Entrée invalide. Veuillez saisir un nombre.\n");

                        }
                    }
                }
                SendMessage("Fin de la partie");
            }


        }
        catch (Exception ex)
        {
            LogInformation($"Erreur lors du traitement du client : {ex.Message}");
        }
        finally
        {
            nbJoueur--;
            LogInformation($"Déconnexion du joueur.Il y a " + nbJoueur + " connecté");
            clientSocket.Close();
        }

    }

    //Fonction permettant de récupérer la réponse avec un Timeout 
    Tuple<int, string> ReceiveDataWithTimeout(int timeoutMilliseconds)
    {
        this.clientSocket.ReceiveTimeout = timeoutMilliseconds;
        byte[] buffer = new byte[1024];

        try
        {
            int bytesRead = this.clientSocket.Receive(buffer);
            return Tuple.Create(bytesRead, Encoding.ASCII.GetString(buffer, 0, bytesRead));
        }
        catch (SocketException)
        {
            if (this.nbNonReponse++ > 4) return Tuple.Create((int)errorReception.DepassementAbsenceReponseMax, "dépassement non réponse");
            LogInformation($"{this.pseudo} : nombre de non reponse {this.nbNonReponse}");
            return Tuple.Create((int)errorReception.AbsenceReponse, string.Empty);
        }
    }

    bool AnalyseNumber(int nb) // vrai si nombre trouvé, Faux sinon
    {
        LoggerFile.Instance.Log($"{pseudo} a proposé : {nb}");
        if (nb == secretNumber) //nombre trouvé
        {
            SendMessage($"Félicitations!{pseudo}  Vous avez deviné le bon nombre." + secretNumber);
            return true;
        }
        else // nombre non trouvé
        {
            string message = nb < this.secretNumber ? "trop bas." : "Trop haut.";
            SendMessage(nb + " est " + message + ". Essayez encore ?");
        }
        return false;
    }
