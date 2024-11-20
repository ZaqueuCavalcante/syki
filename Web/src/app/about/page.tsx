"use client"

import { useState } from "react";

export default function About() {
    const [value, setValue] = useState(0);

    const increment = () => {
        setValue(old => old + 1);
    }

    return (
        <div className="grid grid-rows-[20px_1fr_20px] items-center justify-items-center min-h-screen p-8 pb-20 gap-16 sm:p-20">
            <div className="flex gap-4 items-center flex-col sm:flex-row">
                <h5>ABOUT</h5>
                <button onClick={increment}>CLICKED: {value}</button>
            </div>
        </div>
    );
}
