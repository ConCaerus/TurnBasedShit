using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInputReader : MonoBehaviour {
    const int autoFillThresh = 75;


    public delegate void func(string s);

    public IEnumerator reader(func funcRunsAfterEveryUpdate, func funcRunsAtEnd, string prev = "", List<int> prevPressed = null, int cycleTillAuto = 0) {
        if(prevPressed == null)
            prevPressed = new List<int>();

        //  if none of the inputs that end the renaming process are pressed
        if(!(Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || Input.GetKey(KeyCode.Return))) {
            var arr = prev.ToCharArray();
            bool hasInput = false;

            //  removes the unpressed characters in prevPressed
            for(int i = prevPressed.Count - 1; i >= 0; i--) {
                if(!Input.GetKey((KeyCode)prevPressed[i])) {
                    prevPressed.RemoveAt(i);
                    i--;
                }
            }
            //  if the user backspaces
            if(Input.GetKey(KeyCode.Backspace)) {
                bool hasPressedBackspace = false;
                foreach(var i in prevPressed) {
                    if(i == (int)KeyCode.Backspace) {
                        hasPressedBackspace = true;
                        break;
                    }
                }
                if(((!hasPressedBackspace && cycleTillAuto == 0) || (hasPressedBackspace && prevPressed.Count == 1 && cycleTillAuto > autoFillThresh)) && !string.IsNullOrEmpty(prev)) {
                    hasInput = false;
                    prev = "";
                    for(int i = 0; i < arr.Length - 1; i++) {
                        prev += arr[i];
                    }

                    if(!hasPressedBackspace)
                        prevPressed.Add((int)KeyCode.Backspace);
                }
            }

            //  if the user adds to the string
            else {
                char next = '?';

                //  if the user enters a letter
                bool hasPressedLetter = false;
                for(int i = 97; i < 123; i++) {
                    foreach(var p in prevPressed) {
                        if(p == i) {
                            hasPressedLetter = true;
                            break;
                        }
                    }
                    if(Input.GetKey((KeyCode)i) && !(next != '?' && hasPressedLetter) && ((!hasPressedLetter && cycleTillAuto == 0) || (hasPressedLetter && cycleTillAuto > autoFillThresh))) {
                        if(!hasPressedLetter) {
                            prevPressed.Add(i);
                        }
                        hasInput = true;
                        next = (char)i;
                    }
                }

                //  if the user enters a number
                bool hasPressedNumber = false;
                for(int i = 48; i < 58; i++) {
                    foreach(var p in prevPressed) {
                        if(p == i) {
                            hasPressedNumber = true;
                            break;
                        }
                    }
                    if(Input.GetKey((KeyCode)i) && !(next != '?' && hasPressedNumber) && ((!hasPressedNumber && cycleTillAuto == 0) || (hasPressedNumber && cycleTillAuto > autoFillThresh))) {
                        if(!hasPressedNumber)
                            prevPressed.Add(i);
                        hasInput = true;
                        next = (char)i;
                    }
                }

                //  if the user spaces
                bool hasPressedSpace = false;
                if(Input.GetKey(KeyCode.Space)) {
                    foreach(var i in prevPressed) {
                        if(i == (int)KeyCode.Space) {
                            hasPressedSpace = true;
                            break;
                        }
                    }
                    if(!(next != '?' && hasPressedSpace) && ((!hasPressedSpace && cycleTillAuto == 0) || (hasPressedSpace && cycleTillAuto > autoFillThresh))) {
                        if(!hasPressedSpace)
                            prevPressed.Add((int)KeyCode.Space);
                        hasInput = true;
                        next = ' ';
                    }
                }

                if(hasInput) {
                    if((next >= 97 && next < 123) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                        next = (char)(next - 32);
                    prev += next;
                }
            }

            if(!hasInput && prevPressed.Count == 0) {
                cycleTillAuto = 0;
            }
            else
                cycleTillAuto++;
            yield return new WaitForEndOfFrame();

            funcRunsAfterEveryUpdate(prev);
            StartCoroutine(reader(funcRunsAfterEveryUpdate, funcRunsAtEnd, prev, prevPressed, cycleTillAuto));
        }
        //  end the loop
        else {
            funcRunsAfterEveryUpdate(prev);
            funcRunsAtEnd(prev);
            yield return prev;
        }
    }
}
