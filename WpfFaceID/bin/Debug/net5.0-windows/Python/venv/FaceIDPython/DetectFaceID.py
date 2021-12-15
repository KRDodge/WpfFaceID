import cv2
import timeit
import os
import dotnet
from PIL import Image

class DetectFaceID:

    cascade = cv2.CascadeClassifier('haarcascade_frontalface_alt.xml')

    file_count = 0;

    def ReadVideoFile(self, filePath, id):
        cam = cv2.VideoCapture(filePath)
        DetectFaceID.file_count = len([name for name in os.listdir('dataset/' + str(id) + '/') if os.path.isfile(name)])
        if(DetectFaceID.file_count < 11):
            DetectFaceID.videoDetector(cam, id)

    def ReadImageFile(self, filePath, id):
        img = cv2.resize(img, dsize=None, fx=0.5,fy=0.5)
        DetectFaceID.file_count = len([name for name in os.listdir('dataset/' + str(id) + '/') if os.path.isfile(name)])
        if(DetectFaceID.file_count < 11):
            DetectFaceID.imageDetctor(img, id)

    def videoDetector(self, cam, id):
        while True:
            img = cam.read()
            gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
            faces = DetectFaceID.cascade.detectMultiScale(gray, 1.3, 5)

            for (x, y, w, h) in faces:
                cv2.rectangle(img, (x, y), (x + w, y + h), (255, 0, 0), 2)
                DetectFaceID.file_count += 1
                cv2.imwrite("dataset/" + str(id) + '/' + str(DetectFaceID.file_count) + ".jpg", gray[y:y + h, x:x + w])
                cv2.imshow('image', img)
            k = cv2.waitKey(100) & 0xff
            if k == 27:
                break
            elif DetectFaceID.file_count >= 10:
                break

    def imageDetctor(self, filePath, id):
        img = Image.open(filePath)
        gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        faces = DetectFaceID.cascade.detectMultiScale(gray, 1.3, 5)

        for (x, y, w, h) in faces:
            cv2.rectangle(img, (x, y), (x + w, y + h), (255, 0, 0), 2)
            DetectFaceID.file_count += 1
            cv2.imwrite("dataset/" + str(id) + '/' + str(DetectFaceID.file_count) + ".jpg", gray[y:y + h, x:x + w])
            cv2.imshow('image', img)

        # 사진 출력
        cv2.imshow('facenet', img)
        cv2.waitKey(27)

