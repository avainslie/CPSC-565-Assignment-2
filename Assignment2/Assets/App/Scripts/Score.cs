using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Players{
    public class Score : MonoBehaviour
    {

        // Reference to text mesh pro for the score
        public TextMeshProUGUI countTextG;
        public TextMeshProUGUI countTextS;
        public TextMeshProUGUI whoWon;

        private int countG;
        private int countS;
        public bool gameOver;

        // Start is called before the first frame update
        void Start()
        {
            countG = 0;
            countS = 0;
            setCountText(countG);
            setCountText(countS);
            whoWon.text = "";
            gameOver = false;
        }

        // From Unity Roll-a-ball tutorial
        public void setCountText(int count){
            countTextG.text = "Gryffindor count: " + countG.ToString();
            countTextS.text = "Slytherin count: " + countS.ToString();
        }

        public void increaseScoreG(int points){
            countG+= points;
            if(!win()){
                setCountText(countG);
            }
        }

        public void increaseScoreS(int points){
            countS+= points;
            if(!win()){
                setCountText(countS);
            }  
        }

        private bool win(){
            if (countG == 100){
                whoWon.text = "Gryffindor won!!";
                countTextG.text = "Gryffindor count: 100";
                gameOver = true;
                return true;
            }
            else if (countS == 100){
                whoWon.text = "Slytherin won!!";
                countTextS.text = "Slytherin count: 100";
                gameOver = true;
                return true;
            }
            return false;
        }
    }

}
