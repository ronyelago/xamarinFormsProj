﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        private List<PersonList> _listOfPeople;
        public List<PersonList> ListOfPeople { get { return _listOfPeople; } set { _listOfPeople = value; base.OnPropertyChanged(); } }
        public MasterPage()
        {
            InitializeComponent();



            //var sList = new PersonList()
            //{
            //    new MasterPageItem() { Title = "Atribuição de Cracha", TargetType = typeof(AtribuicaoCracha) },
            //    new MasterPageItem() { Title = "Recebimento de EPI", TargetType = typeof(RecebimentoItens) }

            //};

            //sList.Heading = "Operação";

            //var list = new List<PersonList>()
            //{
            //    sList
            //};

            //listView.ItemsSource = list;

            nameUsuario.Text = UsuarioLogado.DadosUsuario[0].Nome;

            var masterPageItems = new List<MasterPageItem>();

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Atribuir Cracha",
                TargetType = typeof(AtribuicaoCracha)
            });



            masterPageItems.Add(new MasterPageItem
            {
                Title = "Movimentação de Estoque",
                TargetType = typeof(Page2)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Recebimento",
                TargetType = typeof(RecebimentoItens)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Distribuição de EPI",
                TargetType = typeof(DistriEpi)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Envio para Teste",
                TargetType = typeof(EnvioTeste)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Recebimento de Itens Testado",
                TargetType = typeof(RecebimentoTeste)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Envio para Higienização",
                TargetType = typeof(EnvioHigienizacao)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Recebimento da Higienização",
                TargetType = typeof(RecHigienizacao)
            });


            masterPageItems.Add(new MasterPageItem
            {
                Title = "Fiscalização",
                TargetType = typeof(Inspecao)
            });

            //masterPageItems.Add(new MasterPageItem
            //{
            //    Title = "Inspeção",
            //    TargetType = typeof(InspVisual)
            //});

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Manutenção de EPI",
                TargetType = typeof(ManEpi)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Devolução de EPI",
                TargetType = typeof(DevolucaoEpi)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Descarte de EPI",
                TargetType = typeof(DescrtItem)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Registrar Senha",
                TargetType = typeof(RegistrarSenha)
            });


            masterPageItems.Add(new MasterPageItem
            {
                Title = "Consultar EPI",
                TargetType = typeof(CnEPI)
            });

            listView.ItemsSource = masterPageItems;

        }
    }
}