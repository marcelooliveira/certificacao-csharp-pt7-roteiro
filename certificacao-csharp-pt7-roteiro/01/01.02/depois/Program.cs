using System;
using System.Collections.Generic;
using System.Reflection;

namespace _01_02
{
    class Program
    {
        //static void CampainhaTocou1()
        //{
        //    Console.WriteLine("A campainha tocou.");
        //}

        //static void CampainhaTocou2()
        //{
        //    Console.WriteLine("A campainha tocou.");
        //}

        //static void CampainhaTocou1(object sender, EventArgs e)
        //{
        //    Console.WriteLine("A campainha tocou.");
        //}

        //static void CampainhaTocou2(object sender, EventArgs e)
        //{
        //    Console.WriteLine("A campainha tocou.");
        //}

        //static void CampainhaTocou1(object sender, CampainhaEventArgs e)
        //{
        //    Console.WriteLine($"método CampainhaTocou1() foi chamado, apartamento {e.Apartamento}.");
        //}

        //static void CampainhaTocou2(object sender, CampainhaEventArgs e)
        //{
        //    Console.WriteLine($"método CampainhaTocou2() foi chamado, apartamento {e.Apartamento}.");
        //}

        static void CampainhaTocou1(object source, CampainhaEventArgs e)
        {
            Console.WriteLine("método CampainhaTocou1() foi chamado");
            Console.WriteLine("Apartamento: {0}", e.Apartamento);
            throw new Exception("Erro em CampainhaTocou1");
        }

        static void CampainhaTocou2(object source, CampainhaEventArgs e)
        {
            Console.WriteLine("método CampainhaTocou2() foi chamado");
            Console.WriteLine("Apartamento: {0}", e.Apartamento);
            throw new Exception("Erro em CampainhaTocou2");
        }

        static void Main(string[] args)
        {
            //Campainha campainha = new Campainha();
            //campainha.OnCampainhaTocou += CampainhaTocou1;
            //campainha.OnCampainhaTocou += CampainhaTocou2;

            //Console.WriteLine("Chamando campainha.Tocar()");
            //campainha.Tocar();

            //Console.ReadKey();


            //Campainha campainha = new Campainha();
            //campainha.OnCampainhaTocou += CampainhaTocou1;
            //campainha.OnCampainhaTocou += CampainhaTocou2;
            //Console.WriteLine("Chamando campainha.Tocar()");
            //campainha.Tocar();
            //campainha.OnCampainhaTocou -= CampainhaTocou1;
            //Console.WriteLine("Chamando campainha.Tocar()");
            //campainha.Tocar();
            //Console.ReadKey();

            //Campainha campainha = new Campainha();
            //campainha.OnCampainhaTocou += CampainhaTocou1;
            //campainha.OnCampainhaTocou += CampainhaTocou2;
            //Console.WriteLine($"Chamando campainha.Tocar(202)");
            //campainha.Tocar("202");
            //campainha.OnCampainhaTocou -= CampainhaTocou1;
            //Console.WriteLine($"Chamando campainha.Tocar(104)");
            //campainha.Tocar("104");
            //Console.ReadKey();

            Campainha campainha = new Campainha();
            campainha.OnCampainhaTocou += CampainhaTocou1;
            campainha.OnCampainhaTocou += CampainhaTocou2;
            try
            {
                campainha.Tocar("202");
            }
            catch (AggregateException agg)
            {
                foreach (Exception ex in agg.InnerExceptions)
                    Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }

    //class Campainha
    //{
    //    public Action OnCampainhaTocou { get; set; }

    //    public void Tocar()
    //    {
    //        if (OnCampainhaTocou != null)
    //        {
    //            OnCampainhaTocou();
    //        }
    //    }
    //}

    //class Campainha
    //{
    //    public event Action OnCampainhaTocou = delegate { };
    //    public void Tocar()
    //    {
    //        OnCampainhaTocou();
    //    }
    //}

    //class Campainha
    //{
    //    public event EventHandler OnCampainhaTocou = delegate { };
    //    public void Tocar()
    //    {
    //        OnCampainhaTocou(this, EventArgs.Empty);
    //    }
    //}

    class CampainhaEventArgs : EventArgs
    {
        public string Apartamento { get; set; }
        public CampainhaEventArgs(string apartamento)
        {
            Apartamento = apartamento;
        }
    }

    //class Campainha
    //{
    //    public event EventHandler<CampainhaEventArgs> OnCampainhaTocou = delegate { };
    //    public void Tocar(string apartamento)
    //    {
    //        OnCampainhaTocou(this, new CampainhaEventArgs(apartamento));
    //    }
    //}

    class Campainha
    {
        public event EventHandler<CampainhaEventArgs> OnCampainhaTocou = delegate { };
        public void Tocar(string apartamento)
        {
            List<Exception> listaExcecoes = new List<Exception>();
            foreach (Delegate handler in OnCampainhaTocou.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(this, new CampainhaEventArgs(apartamento));
                }
                catch (TargetInvocationException e)
                {
                    listaExcecoes.Add(e.InnerException);
                }
            }

            if (listaExcecoes.Count > 0)
                throw new AggregateException(listaExcecoes);
        }
    }
}
