import React from 'react'

export default class SearchUsersLayout extends React.Component {

    constructor(props) {
        super(props)

        this.state = {
            nicknameGuess: '',
            nicknames: [],
            selectedNicknameId: ''
        }

        this.updateNicknameInput = this.updateNicknameInput.bind(this)
        this.handleSelectedNickname = this.handleSelectedNickname.bind(this)
    }

    async sendGetNicknameGuess() {
        const req = {
            method: 'GET',
            'Content-Type': 'application/json'
        }

        let nicknames = await fetch(`/users/options/${this.state.nicknameGuess}`, req)
        nicknames = await nicknames.json()

        const formated = this.formatNicknames(nicknames)
        this.setState({nicknames: formated})
    }

    formatNicknames(data) {
        return data.map(i => <option value={i.item2}> {i.item1} </option>)
    }

    updateNicknameInput(e) {
        this.setState({nicknameGuess: e.target.value}, () => {
            if (this.state.nicknameGuess.length >= 2) {
                this.sendGetNicknameGuess(this.state.nicknameGuess)
            }
        })
    }

    handleSelectedNickname(e) {
        this.setState({
            selectedNicknameId: e.target.value,
            nicknameGuess: ''
        }, () => {
            window.location.href = `/users/show/${this.state.selectedNicknameId}`
        })
    }

    render() {
        return <div>
            <label>Search Users</label>
            <input onChange={this.updateNicknameInput} value={this.state.nicknameGuess} type="text"/>
            <select value={this.state.selectedNickname} onClick={this.handleSelectedNickname}>
                {this.state.nicknames}
            </select>
        </div>
    }

}