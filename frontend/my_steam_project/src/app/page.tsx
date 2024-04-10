import Image from "next/image";
import GameList from "./Games/GameList";
import { useStore } from "./HooksManagement/basketItemState";
import { getBasketItems } from "./api/basket/basketActions";

export default async function Home() {

  return (
    <div className="container">
     <main className=" flex min-h-screen flex-col items-center justify-between p-24" >
      <GameList></GameList>
     </main>
     </div>
  );
}
