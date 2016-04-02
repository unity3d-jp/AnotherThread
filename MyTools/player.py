import Image
import colorsys
import ImageDraw
import math

def convert(img):
    rgb_img = img.convert('RGB')
    img = Image.new('RGBA', rgb_img.size, (0x00,0x00,0x00,0xff))
    draw = ImageDraw.Draw(img)

    x,y = rgb_img.size
    for i in range(0,x):
        for j in range(0,y):
            r, g, b = rgb_img.getpixel((i, j))
            h,s,v = colorsys.rgb_to_hsv(r/255.,g/255.,b/255.)
            if (s > 0.1):
                draw.point((i, j), (r, g, b, 0xff))
            else:
                draw.point((i, j), (255-r, 255-g, 255-b, 0xff))
                
    return img

def make_image():
    img = Image.open('ship_fighter.jpg', 'r')
    img = convert(img)
    img.save('black_fighter.png')

if __name__ == '__main__':
    make_image()

#EOF
