using term3d;

var r = new Render();
r.Init();

(int x, int y, int z) CameraLocation = (0,0,0);

// main loop
while (true)
{
  if (Console.KeyAvailable)
  {
    ReadKey();
  }
  else
  {
    r.CameraRaycast(CameraLocation);
  }
}

void ReadKey()
{
  var keyInfo = Console.ReadKey(true);
  switch (keyInfo.Key)
  {
    case ConsoleKey.W:
      CameraLocation.z++;
      break;
    case ConsoleKey.S:
      CameraLocation.z--;
      break;
    case ConsoleKey.A:
      CameraLocation.x--;
      break;
    case ConsoleKey.D:
      CameraLocation.x++;
      break;
    case ConsoleKey.R:
      CameraLocation.y++;
      break;
    case ConsoleKey.F:
      CameraLocation.y--;
      break;
  }
}


