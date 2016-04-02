import sys
import Image
import colorsys
import ImageDraw
import math

def convert(img):
    rgb_img = img.convert('RGBA')
    img = Image.new('RGBA', rgb_img.size, (0x00,0x00,0x00,0xff))
    draw = ImageDraw.Draw(img)

    x,y = rgb_img.size
    for i in range(0,x):
        for j in range(0,y):
            r, g, b, a = rgb_img.getpixel((i, j))
            h,s,v = colorsys.rgb_to_hsv(r/255.,g/255.,b/255.)
            r0 = max(0xff, r * 4);
            g0 = max(0xff, g * 4);
            b0 = max(0xff, b * 4);
            draw.point((i, j), (r0, g0, b0, a))
                
    return img

def make_image(file):
    img = Image.open(file, 'r')
    img = convert(img)
    img.save('new/'+file)

if __name__ == '__main__':
    for arg in sys.argv:
        print arg
        if arg.endswith('.png'):
            make_image(arg)

#EOF
