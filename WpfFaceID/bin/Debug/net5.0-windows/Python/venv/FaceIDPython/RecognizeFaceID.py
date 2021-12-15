import cv2
import numpy as np
import os

class RecognizeFaceID:

    min_confidence = 50
    recognizer = cv2.face.LBPHFaceRecognizer_create()
    faceCascade = cv2.CascadeClassifier('haarcascade_frontalface_alt.xml')

    def reconizeFaceByVideo(self, id, cam):
        cam = cv2.VideoCapture(0)
        RecognizeFaceID.recognizer.read("ymldata/" + str(id) + '.yml')

        if(cam.isOpened() == false):
            return False;

        while True:
            ret, img = cam.read()
            gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

            faces = RecognizeFaceID.faceCascade.detectMultiScale(gray,
                                                                 scaleFactor=1.2,
                                                                 minNeighbors=5,
                                                                 minSize = (10,10))

            for (x, y, w, h) in faces:
                cv2.rectangle(img, (x, y), (x + w, y + h), (0, 255, 0), 2)
                result, confidence = RecognizeFaceID.recognizer.predict(gray[y:y + h, x:x + w])

                if (confidence < RecognizeFaceID.min_confidence):
                    result = True;
                    confidence = "  {0}%".format(round(100 - confidence))
                    break;
                elif(confidence > RecognizeFaceID.min_confidence & confidence <100):
                    result = False;
                    confidence = "  {0}%".format(round(100 - confidence))
                else:
                    result = False;
                    confidence = "  {0}%".format(round(100 - confidence))

                cv2.putText(img, str(result), (x + 5, y - 5), font, 1, (255, 255, 255), 2)
                cv2.putText(img, str(confidence), (x + 5, y + h - 5), font, 1, (255, 255, 0), 1)

            cv2.imshow('camera', img)
            k = cv2.waitKey(10) & 0xff  # 비디오 종료 시 'ESC' 누르기
            if k == 27:
                break
            elif(confidence > 50):
                break;

        return result;


    def recognizeFaceByImage(self, id, img):
        RecognizeFaceID.recognizer.read("ymldata/" + str(id) + '.yml')

        if (cam.isOpened() == false):
            return False;

        while True:
            gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
            faces = RecognizeFaceID.faceCascade.detectMultiScale(gray,
                                                                 scaleFactor=1.2,
                                                                 minNeighbors=5,
                                                                 minSize=(10, 10))

            for (x, y, w, h) in faces:
                cv2.rectangle(img, (x, y), (x + w, y + h), (0, 255, 0), 2)
                result, confidence = RecognizeFaceID.recognizer.predict(gray[y:y + h, x:x + w])

                if (confidence < RecognizeFaceID.min_confidence):
                    result = True;
                    confidence = "  {0}%".format(round(100 - confidence))
                    break;
                elif(confidence > RecognizeFaceID.min_confidence & confidence <100):
                    result = False;
                    confidence = "  {0}%".format(round(100 - confidence))
                else:
                    result = False;
                    confidence = "  {0}%".format(round(100 - confidence))

                cv2.putText(img, str(result), (x + 5, y - 5), font, 1, (255, 255, 255), 2)
                cv2.putText(img, str(confidence), (x + 5, y + h - 5), font, 1, (255, 255, 0), 1)

            return result;