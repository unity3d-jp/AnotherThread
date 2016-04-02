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
            draw.point((i, j), (0xff,0xff,0xff,0x0))

    for i in range(0,x):
        draw.point((i, rad-6), (0xff,0xff,0xff,0xff))
        draw.point((i, rad-5), (0xff,0xff,0xff,0xff))
        draw.point((i, rad-4), (0xff,0xff,0xff,0xff))
        draw.point((i, rad-3), (0xff,0xff,0xff,0xff))
        draw.point((i, rad-2), (0xff,0xff,0xff,0xff))
        draw.point((i, rad-1), (0xff,0xff,0xff,0xff))
        draw.point((i, rad+0), (0xff,0xff,0xff,0xff))
        draw.point((i, rad+1), (0xff,0xff,0xff,0xff))
        draw.point((i, rad+2), (0xff,0xff,0xff,0xff))
        draw.point((i, rad+3), (0xff,0xff,0xff,0xff))
        draw.point((i, rad+4), (0xff,0xff,0xff,0xff))
        draw.point((i, rad+5), (0xff,0xff,0xff,0xff))
        draw.point((i, rad+6), (0xff,0xff,0xff,0xff))
        draw.point((i, rad+7), (0xff,0xff,0xff,0xff))

    for i in range(0,y):
        draw.point((rad-6, i), (0xff,0xff,0xff,0xff))
        draw.point((rad-5, i), (0xff,0xff,0xff,0xff))
        draw.point((rad-4, i), (0xff,0xff,0xff,0xff))
        draw.point((rad-3, i), (0xff,0xff,0xff,0xff))
        draw.point((rad-2, i), (0xff,0xff,0xff,0xff))
        draw.point((rad-1, i), (0xff,0xff,0xff,0xff))
        draw.point((rad+0, i), (0xff,0xff,0xff,0xff))
        draw.point((rad+1, i), (0xff,0xff,0xff,0xff))
        draw.point((rad+2, i), (0xff,0xff,0xff,0xff))
        draw.point((rad+3, i), (0xff,0xff,0xff,0xff))
        draw.point((rad+4, i), (0xff,0xff,0xff,0xff))
        draw.point((rad+5, i), (0xff,0xff,0xff,0xff))
        draw.point((rad+6, i), (0xff,0xff,0xff,0xff))
        draw.point((rad+7, i), (0xff,0xff,0xff,0xff))

#    draw.line((0, rad, x, rad), fill=(0xff,0xff,0xff,0xff))

    for i in range(0,x):
        for j in range(0,y):
            dist = math.sqrt((i - cx)*(i - cx) + (j - cy)*(j - cy))
            if rad-40 < dist and dist < rad-26:
                val = 0xff;
                draw.point((i, j), (0xff,0xff,0xff,int(val)))

            if (dist < rad-40):
                draw.point((i, j), (0xff,0xff,0xff,0x00))

    return img

def make_image(bgcolor, filename):
    w = 256
    img = Image.new('RGBA', (w,w), bgcolor)
    img = drawing(img)
    img = img.resize((w/2,w/2), Image.ANTIALIAS)
    img = img.resize((w/4,w/4), Image.ANTIALIAS)
    img.save(filename)

if __name__ == '__main__':
    bgcolor=(0xff,0xff,0xff,0x00)
    filename = "cursor.png"
    make_image(bgcolor, filename)

#EOF
