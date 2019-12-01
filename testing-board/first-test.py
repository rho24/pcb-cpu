import board
import busio
import digitalio
from adafruit_bus_device.i2c_device import I2CDevice

def arrayToBytes(a):
    return bytearray(bytes(a))

oe_n = digitalio.DigitalInOut(board.C0)
reset_n = digitalio.DigitalInOut(board.C1)

oe_n.direction = digitalio.Direction.OUTPUT
reset_n.direction = digitalio.Direction.OUTPUT

reset_n.value = 1
oe_n.value = 0

i2c = busio.I2C(board.SCL, board.SDA)
device = I2CDevice(i2c, 32)

slaveAddress = 0x40


deviceIdBytes = bytearray(3)
device.write_then_readinto(arrayToBytes([
    0b11111000,
    (slaveAddress << 1) | 0x00,
    0b11111001
    ]), deviceIdBytes)
print(deviceIdBytes)


readBytes = bytearray(1)
# device.write_then_readinto(bytearray(bytes([
#     (slaveAddress << 1) | 0x00,
#     0b00000000,
#     (slaveAddress << 1) | 0x01])), readBytes)
# print(readBytes)
