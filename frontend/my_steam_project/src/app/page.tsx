import Image from "next/image";
import GameList from "./Games/GameList";

export default function Home() {
  return (
    <div className="container">
     <main className=" flex min-h-screen flex-col items-center justify-between p-24" >
      <GameList></GameList>
     </main>
     </div>
  );
}
