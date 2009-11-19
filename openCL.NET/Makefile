TARGET=openCL.NET.dll

all: $(TARGET)
clean:
	rm -f $(TARGET)

$(TARGET): *.cs
	gmcs -out:$@ -optimize+ -target:library *.cs
