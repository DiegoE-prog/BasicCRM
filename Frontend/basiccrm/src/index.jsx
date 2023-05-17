import React from "react"
import ReactDOM from "react-dom/client"
import "./index.css"
import DirectoryIndex from "./DirectoryIndex"
import Header from "./Components/Layout/Header"
import Footer from "./Components/Layout/Footer"
import { BrowserRouter, Route, Routes } from "react-router-dom"
import AddressIndex from "./Components/Address/AddressIndex"
import AddAddress from "./Components/Address/AddAddress"
import EditAddress from "./Components/Address/EditAddress"
import ClientIndex from "./Components/Client/ClientIndex"
import AddClient from "./Components/Client/AddClient"
import EditClient from "./Components/Client/EditClient"

const root = ReactDOM.createRoot(document.getElementById("root"))
root.render(
	<div className="container">
		<BrowserRouter>
			<Header />
			<Routes>
				<Route path="/" element={<DirectoryIndex />}></Route>
				<Route path="/Address/AddressIndex" element={<AddressIndex />}></Route>
				<Route path="/Address/AddAddress" element={<AddAddress />}></Route>
				<Route path="/Address/EditAddress/:id" element={<EditAddress />}></Route>
				<Route path="/Client/ClientIndex" element={<ClientIndex />}></Route>
				<Route path="/Client/AddClient" element={<AddClient />}></Route>
				<Route path="/CLient/EditClient/:id" element={<EditClient />}></Route>
			</Routes>
			<Footer />
		</BrowserRouter>
	</div>
)
