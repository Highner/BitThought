﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepThought
{
    public class BigBrain
    {
        #region constructor
        public BigBrain()
        { }
        public BigBrain(int numberofinputneurons, int[] layers, int numberofoutputneurons, Func<decimal,decimal> activationfunction)
        {
            LayerDistribution = layers;

            NumberOfInputNeurons = numberofinputneurons;

            NumberOfOutputNeurons = numberofoutputneurons;

            ActivationFunction = activationfunction;
        }
        #endregion

        #region public methods
        public void CreateNetwork()
        {
            CreateInputNeurons();

            CreateOutputNeurons();

            CreateHiddenNeurons();
        }
        #endregion

        #region private methods
        private void CreateInputNeurons()
        {
            InputNeurons = new List<InputNeuron>();
            for (int i = 1; i <= NumberOfInputNeurons; i++)
            {
                InputNeurons.Add(new InputNeuron(ActivationFunction));
            }
        }

        private void CreateOutputNeurons()
        {
            OutputNeurons = new List<OutputNeuron>();
            for (int i = 1; i <= NumberOfOutputNeurons; i++)
            {
                OutputNeurons.Add(new OutputNeuron(ActivationFunction));
            }
        }

        private void CreateHiddenNeurons()
        {
            HiddenNeurons = new List<HiddenNeuron>();
            for (int i = 1; i <= NumberOfLayers; i++)
            {
                for (int x = 1; x <= LayerDistribution[i]; x++)
                {
                    HiddenNeurons.Add(new HiddenNeuron(ActivationFunction, i));
                }
            }
        }

        private void CreateSynapses()
        {
            Synapses = new List<Synapse>();
            //connect first layer to inputneurons
            foreach(InputNeuron neuron in InputNeurons)
            {
                foreach (HiddenNeuron hiddenneuron in HiddenNeurons.Where(x => x.Layer == 1))
                {
                    Synapses.Add(new Synapse(neuron, hiddenneuron));
                }
            }

            //connect hidden layers
            for (int i = 1; i < NumberOfLayers; i++)
            {
                foreach(HiddenNeuron sourceneutron in HiddenNeurons.Where(x => x.Layer == i))
                {
                    foreach (HiddenNeuron targetneutron in HiddenNeurons.Where(x => x.Layer == i + 1))
                    {
                        Synapses.Add(new Synapse(sourceneutron, targetneutron));
                    }
                }
            }

            //connect to outputneurons
            foreach (HiddenNeuron sourceneutron in HiddenNeurons.Where(x => x.Layer == NumberOfLayers))
            {
                foreach (OutputNeuron targetneutron in OutputNeurons)
                {
                    Synapses.Add(new Synapse(sourceneutron, targetneutron));
                }
            }
        }
        #endregion

        #region properties
        private List<InputNeuron> InputNeurons { get; set; }

        private List<OutputNeuron> OutputNeurons { get; set; }

        private List<HiddenNeuron> HiddenNeurons { get; set; }

        private List<Synapse> Synapses { get; set; }
        #endregion

        #region fields
        private int NumberOfLayers
        {
            get
            {
                return LayerDistribution.Count();
            }
        }

        private int[] LayerDistribution;

        private int NumberOfInputNeurons;

        private int NumberOfOutputNeurons;

        private Func<decimal, decimal> ActivationFunction;
        #endregion
    }
}
