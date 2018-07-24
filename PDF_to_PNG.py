#https://stackoverflow.com/questions/27826854/python-wand-convert-pdf-to-png-disable-transparent-alpha-channel
import os
import sys
import shutil

from wand.image import Image, Color

def convertPDF(filename, output_path, resolution=300):
    """ Convert a PDF into images.

        All the pages will give a single png file with format:
        {pdf_filename}-{page_number}.png

        The function removes the alpha channel from the image and
        replace it with a white background.
    """
    all_pages = Image(filename=filename, resolution=resolution)
    for i, page in enumerate(all_pages.sequence):
        with Image(page) as img:
            img.format = 'png'
            img.background_color = Color('white')
            img.alpha_channel = 'remove'

            image_filename = os.path.splitext(os.path.basename(filename))[0]
            image_filename = '{}-{}.png'.format(image_filename, i)
            image_filename = os.path.join(output_path, image_filename)
            print("Converted to: ", image_filename)
            img.save(filename=image_filename)
