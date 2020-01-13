using IronOcr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BotProject
{
    class Character
    {

        public StateValue Casting, HasTarget, InRange, InCombat, InMeleeRange, PlayerTargeted, TargetInCombat;
        public List<StateValue> StateValues = new List<StateValue>();

        public static Character Instance;

        // OCR read values to be set
        public OcrResult PlayerTextResult;
        public Vector2 PlayerPosition;
        public double PlayerX;
        public double PlayerY;
        public double PlayerDirection;


        public Character()
        {
            Instance = this;

            Casting = new StateValue(1, 2);
            HasTarget = new StateValue(5, 2);
            InRange = new StateValue(9, 2);
            InCombat = new StateValue(13, 2);
            InMeleeRange = new StateValue(17, 2);
            PlayerTargeted = new StateValue(26, 2);
            TargetInCombat = new StateValue(31, 2);
        }

        public void GetStates()
        {
            for (int i = 0; i < StateValues.Count; i++)
            {
                StateValues[i].value = ImageProcessing.Instance.GetColorAt(StateValues[0].X, StateValues[i].Y).R > 0.2f;
            }
        }
    }
    public class StateValue
    {
        public int X;
        public int Y;
        public bool value;

        public StateValue(int x, int y)
        {
            X = x;
            Y = y;
            Character.Instance.StateValues.Add(this);
        }
    }
}
