// See https://aka.ms/new-console-template for more information

using term3d;

var r = new Render();
r.Init();
int i = 0;
while (i < 10){
  r.RenderFrame();
    i++;
}
