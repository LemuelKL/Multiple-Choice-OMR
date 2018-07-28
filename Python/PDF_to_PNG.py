#https://stackoverflow.com/questions/27826854/python-wand-convert-pdf-to-png-disable-transparent-alpha-channel
import os
import sys
import shutil

from wand.image import Image, Color

def convertPDF(filename, output_path, resolution=300):
    pngNames = []
    all_pages = Image(filename=filename, resolution=resolution)
    for i, page in enumerate(all_pages.sequence):
        with Image(page) as img:
            img.format = 'png'
            img.background_color = Color('white')
            img.alpha_channel = 'remove'

            image_filename = os.path.splitext(os.path.basename(filename))[0]
            image_filename = '{}-{}.png'.format(image_filename, i)
            pngNames.append(image_filename)
            image_filename = os.path.join(output_path, image_filename)
            print("Converted to: ", image_filename)
            img.save(filename=image_filename)
    return [i+1, pngNames]
    