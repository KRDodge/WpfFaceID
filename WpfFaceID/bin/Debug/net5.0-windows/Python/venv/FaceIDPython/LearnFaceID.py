import cv2
import numpy as np
from PIL import Image
import os


class LearnFaceID:
    path = ''
    recognizer = cv2.face.LBPHFaceRecognizer_create()
    detector = cv2.CascadeClassifier('./venv/Lib/site-packages/cv2/data/haarcascade_frontalface_alt.xml')

    def learnImage(self, id):
        LearnFaceID.path = 'dataset/' + str(id)
        faces, ids = LearnFaceID.getImageAndLabels()
        LearnFaceID.recognizer.train(faces, np.array(ids))
        fileName = 'ymldata/' + id + '.yml'
        LearnFaceID.recognizer.write(fileName)

    def getImageAndLabels(self):
        imagePaths = [os.path.join(LearnFaceID.path, f) for f in os.listdir(LearnFaceID.path)]
        faceSamples=[]
        ids = []
        for imagePath in imagePaths:
            PIL_img = Image.open(imagePath).convert('L')  # grayscale로 변환
            img_numpy = np.array(PIL_img, 'uint8')
            id = int(imagePath.rsplit('.', 1)[0])
            faces = LearnFaceID.detector.detectMultiScale(img_numpy)

            for (x, y, w, h) in faces:
                faceSamples.append(img_numpy[y:y + h, x:x + w])
                ids.append(id)
        return faceSamples, ids
