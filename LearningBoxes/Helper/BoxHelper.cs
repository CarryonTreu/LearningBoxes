using LearningBoxes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningBoxes.Helper {
    class BoxHelper {

        public static Box[] CreateBoxes() {
            Box[] newBoxes = new Box[5];

            for (int i = 0; i < newBoxes.Length; i++) {
                Box tmpBox = new Box();
                newBoxes[i] = tmpBox;

                //Id
                newBoxes[i].id = i;

                //Date
                DateTime now = DateTime.Now;
                newBoxes[i].creationDate = now;
                newBoxes[i].lastEditDate = now;

                //Days between Test
                switch (i) {
                    case 0:
                        newBoxes[i].daysBetweenTest = 2;
                        break;
                    case 1:
                        newBoxes[i].daysBetweenTest = 7;
                        break;
                    case 2:
                        newBoxes[i].daysBetweenTest = 28;
                        break;
                    case 3:
                        newBoxes[i].daysBetweenTest = 56;
                        break;
                    case 4:
                        newBoxes[i].daysBetweenTest = 90;
                        break;
                    default:
                        return null;
                }
            }
            return newBoxes;
        }
    }
}
