﻿/*
 * information sources:
 * msdn.microsoft.com
 * tutorialspoint.com
 * 
 * Hosted at:
 * https://github.com/Magister-Machinis/CS520 (amongst my other efforts)
 * 
 * John Fromholtz
 * CS520 Fall 2017
 */


namespace SimulationCore
{
    partial class Rider //generic holder for riders and whether they are on a buss or waiting
    {
        bool riding = false;

        public bool getState()
        {
            return riding;
        }

        public void toggleState()
        {
            if (riding == false)
            {
                riding = true;
            }
            else
            {
                riding = false;
            }
        }
    }
}

 