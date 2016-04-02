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
            dist = math.sqrt((i - cx)*(i - cx) + (j - cy)*(j - cy))
            val = 0xff*(1.0 - (dist/rad));
            draw.point((i, j), (0xff,0xff,0xff,int(val)))
    return img

def make_image(screen, bgcolor, filename):
    img = Image.new('RGBA', screen, bgcolor)
    img = drawing(img)
    img.save(filename)

if __name__ == '__main__':
    screen = (64,64)

    bgcolor=(0x00,0x00,0x00,0x00)

    filename = "sphere.png"

    make_image(screen, bgcolor, filename)

#EOF
