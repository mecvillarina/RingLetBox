# RINGLETBOX

RingLetBox is a Token Creation, Vesting, and Locking Platform. RingLetBox aims to protects every cryptocurrency community from rug pulls and traitorous advisors. 

## FEATURES
- Token Creation - Token Creation on Stratis is not a new feature. RingLetBox creates an alternative way to create your token.
- Team Vesting - Prove your commitment to your community by locking your team's tokens
- Community Vesting - Reward the members of your community who has serious involvement or loyalty to your project.
- Token Lock - As ordinary person, the only way to have a diamond hands is to lock your token. 

## USERS
- The `sender` can:
    - create a lock and specify the `recipient` and unlock date of the lock.
    - revoke the lock anytime as long as the recipient doesn't claim it yet regardless if the unlock date has been elapsed.
    - refund the standard token after revoking the lock.

- The `recipient` can:
    - claim the standard token of the lock after the unlock date has been elapsed given the lock was not revoked by the `sender`.

## ARCHITECTURE
![](assets/infrastructure.png)

## LIMITATION:
- Due to technical difficulties on setting up the cirrus testnet node, the network used for this project is **Cirrus Core Private Net**.

## TECHNOLOGIES
- Blockchain/Sidechain Network: **Cirrus Core Private Net**
- Smart Contract: **Cirrus Smart Contracts**
- Web Frontend: **Blazor Web Assembly**
- Web API: **Azure Function Apps**
- Database: **Azure SQL Database**
- Cloud: **Azure**

## DEMO

- Link: **[Web App](https://red-grass-0b98b8f03.azurestaticapps.net)**
- Credentials:
    - Wallet Name: **cirrusdev**
    - Wallet Password: **password**

## GUIDES:
- API Setup Guide: **See [API Setup Guide](app/README.Backend.md)**
- Web Frontend Setup Guide: **See [Web Frontend Setup Guide](app/README.Frontend.md)**
- Smart Contract for LockTokenVault: **See [Smart Contract for LockTokenVault](smart-contracts/LockTokenVault/README.md)**

