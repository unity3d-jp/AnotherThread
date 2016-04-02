import Image
import ImageDraw
import math

def make_image(bgcolor, filename):
    w = 32
    img = Image.new('RGBA', (w,w), bgcolor)
    img.save(filename)

if __name__ == '__main__':
    bgcolor=(0xff,0xff,0xff,0xfe)
    filename = "square.png"
    make_image(bgcolor, filename)

#EOF
