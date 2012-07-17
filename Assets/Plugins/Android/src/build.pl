#!/usr/local/bin/perl

print ("Building Java class...\n");
system ("javac IOIOInterface.java -bootclasspath /android-sdk-mac_86/platforms/android-8/android.jar -d . -classpath /Applications/Unity/Unity.app/Contents/PlaybackEngines/AndroidPlayer/bin/classes.jar");

print ("Creating signature...\n");
system ("javap -s org.AngryAnt.IOIO.IOIOInterface");

print ("Creating jar...\n");
system ("jar cvfM ../IOIOInterface.jar org/");

print ("Cleaning up...\n");
system ("rm -rf libs");
system ("rm -rf obj");
system ("rm -rf org");

print ("Success");