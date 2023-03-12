#!/bin/sh

while true; do (clear > /dev/tty$1); getty 38400 tty$1; done
