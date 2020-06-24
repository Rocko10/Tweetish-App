import React from 'react'

export default class ReactionBtn extends React.Component
{
    constructor(props) {
        super(props)

        this.state = {
            text: ''
        }

        this.sendReacted = this.sendReacted.bind(this)
        this.sendToggleUserTweetReaction = this.sendToggleUserTweetReaction.bind(this)
    }

    componentDidMount() {
        this.sendReacted()
    }

    sendReacted() {
        fetch(`/userTweetReaction/reacted/${this.props.userId}/${this.props.tweet.id}/${this.props.reaction.id}`)
        .then(res => res.json())
        .then(reaction => {
            let text = ''

            if (this.props.reaction.name === 'Heart') {
                if (reaction.reacted) {
                    text = 'Un-Heart'
                } else {
                    text = 'Heart'
                }
            }

            if (this.props.reaction.name === 'Star') {
                if (reaction.reacted) {
                    text = 'Un-Star'
                } else {
                    text = 'Star'
                }
            }

            this.setState({text})
        })
    }

    sendToggleUserTweetReaction(e) {
        const req = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                userId: this.props.userId,
                tweetId: this.props.tweet.id,
                reactionId: this.props.reaction.id
            })
        }

        e.target.disabled = true
        fetch('/userTweetReaction/toggle', req)
        .then(data => {
            if (data.status == 200) {
                alert('Reaction toggled')
            }
        })
    }

    render() {
        return <button onClick={this.sendToggleUserTweetReaction}>
            {this.state.text}
        </button>
    }

}