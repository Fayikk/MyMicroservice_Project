
import { useStore } from "./HooksManagement/basketItemState";
import NavigationBar from "./Nav/NavigationBar";
import "./globals.css"
import HotToastrProvider from "./providers/HotToastrProvider";

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {


  return (
    <html lang="en">
      <body className="bg-orange-100" >
        <HotToastrProvider></HotToastrProvider>
      <NavigationBar></NavigationBar>
        
        <main className="container mx-auto px"  >
        {children}
        </main>
        </body>

    </html>
  );
}
