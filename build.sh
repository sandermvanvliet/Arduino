#!/bin/sh

solutiondir=`pwd -P`
echo $solutiondir

cd "$solutiondir/Solid.Arduino" && xbuild /p:Configuration=Debug /p:DefineConstants="DEBUG=True"

cd "$solutiondir/Degree.Arduino.Test" && xbuild /p:Configuration=Debug /p:DefineConstants="DEBUG=True"
