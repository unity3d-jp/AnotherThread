import Image
import ImageDraw
import math

def drawing(img):
    x,y = img.size
    cx = x/2
    cy = y/2
    if x < y:
        rad = x/2
    else:
        rad = y/2
    draw = ImageDraw.Draw(img)
    for i in range(0,x):
        for j in range(0,y):
            dist = float(abs(i-cx))
            y_ratio = float(j)/float(y)
            #val = 0x80*(1.0 - (dist/rad)) + (0x80-0x1)
            #val = 0xff*(1.0 - (dist/rad)) + (0x0)
            val = (0x80 + ((y_ratio)*0x7f)) * (1.0 - (dist/rad)) + ((1-y_ratio)*0x80-0x1)
            #val = (0x0 + ((y_ratio)*0xff)) * (1.0 - (dist/rad)) + ((1-y_ratio)*0x80-0x1)
            draw.point((i, j), (0xff,0xff,0xff,int(val)))
    return img

def make_image(screen, bgcolor, filename):
    img = Image.new('RGBA', screen, bgcolor)
    img = drawing(img)
    img.save(filename)

if __name__ == '__main__':
    screen = (8,8)

    bgcolor=(0x00,0x00,0x00,0x00)

    filename = "laser.png"

    make_image(screen, bgcolor, filename)

#EOF
