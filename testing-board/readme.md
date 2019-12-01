
# https://learn.adafruit.com/circuitpython-on-any-computer-with-ft232h/gpio
# https://www.mouser.co.uk/datasheet/2/302/PCA9698-1127696.pdf

# Setup
```
sudo apt-get install libusb-1.0
sudo pip install adafruit-circuitpython-lis3dh
sudo pip install pyftdi
```

# first run
```
export BLINKA_FT232H=1
python3 blink-c0.py
```