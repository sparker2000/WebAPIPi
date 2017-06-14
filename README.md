# WebAPIPi
.NET Core Web API Project that provides access to the GPIO pins on a Raspberry Pi 3 Model B V1.5

1. Start by installing Ubuntu 16.04 LTS 'classic' from https://wiki.ubuntu.com/ARM/RaspberryPi
2. # Install the packages necessary for .NET Core
	sudo apt-get -y install libunwind8 libunwind8-dev gettext libicu-dev liblttng-ust-dev libcurl4-openssl-dev libssl-dev uuid-dev
3. 	Get the latest .NET Core ARM release from https://github.com/stevedesmond-ca/dotnet-arm/releases/  Follow the instructions given.
4. Build this project.
5. Move all files in the C:\source\WebAPIPi\WebAPIPi\bin\Debug\netcoreapp2.0\ubuntu.16.04-arm\publish folder to a new directory on your rpi3
6. Navigate to the directory and make the WebAPIPi file executable chmod u+x,o+x WebAPIPi
7. Install WiringPi http://wiringpi.com/download-and-install/
8. To run the web server, make sure you are in the directory and type "sudo ./WebAPIPi".  Sudo is required since we're interacting with GPIO.



This project is a combination of two existing projects:
1. The .NET Core 2 WebAPI project created by Jeremy Lindsay. https://jeremylindsayni.wordpress.com/2017/04/22/hosting-a-net-core-2-web-api-instance-on-the-raspberry-pi-3/
2. The Enterprisecoding Raspberry Pi GPIO Library created by written by Fatih Boy. https://github.com/fatihboy/RPI.GPIO
