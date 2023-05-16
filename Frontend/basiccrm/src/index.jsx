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
			</Routes>
			<Footer />
		</BrowserRouter>
	</div>
)
