Import("env")

def fix_delay():
    import fileinput

    path = env['PROJECT_LIBDEPS_DIR'] + "/" + env['PIOENV'] + "/OneWire/OneWire.h"
    value = '#define delayMicroseconds busy_wait_us\n'

    for line in fileinput.FileInput(path,inplace=1):
        if line.startswith('#define delayMicroseconds busy_wait_us'):
            continue
        elif line.startswith('#include <Arduino.h> '):
            line=line.replace(line,line+value)
        print(line, end='')

fix_delay()