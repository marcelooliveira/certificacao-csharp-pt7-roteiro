using System;

namespace _01_01
{
    class Program
    {
        static void CampainhaTocou1()
        {
            Console.WriteLine("A campainha tocou.");
        }

        static void CampainhaTocou2()
        {
            Console.WriteLine("A campainha tocou.");
        }

        static void Main(string[] args)
        {
            Campainha campainha = new Campainha();
            campainha.OnCampainhaTocou += CampainhaTocou1;
            campainha.OnCampainhaTocou += CampainhaTocou2;

            campainha.Tocar();
            Console.ReadKey();
        }
    }

    class Campainha
    {
        public Action OnCampainhaTocou { get; set; }

        public void Tocar()
        {
            //if (OnCampainhaTocou != null)
            //{
            //    OnCampainhaTocou();
            //}

            OnCampainhaTocou?.Invoke();
        }
    }

}
