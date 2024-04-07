
import NavigationBar from "./Nav/NavigationBar";
import "./globals.css"

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {

  console.log("trigger server")
  return (
    <html lang="en">
      <body className="bg-orange-100" >
      <NavigationBar></NavigationBar>
        
        <main className="container mx-auto px"  >
        {children}
        </main>
        </body>

    </html>
  );
}
